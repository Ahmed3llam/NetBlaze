using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.Domain.Views;
using NetBlaze.SharedKernel.Dtos.Attendance.Request;
using NetBlaze.SharedKernel.Dtos.Attendance.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class AttendService : IAttendService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtBearerService _jwtBearerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Fido2 _fido;
        private readonly IMemoryCache _cache;
        public AttendService(
            IUnitOfWork unitOfWork,
            IJwtBearerService jwtBearerService,
            IHttpContextAccessor httpContextAccessor,
            Fido2 fido,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _jwtBearerService = jwtBearerService;
            _httpContextAccessor = httpContextAccessor;
            _fido = fido;
            _cache = cache;
        }

        public async Task<ApiResponse<AttendUserResponseDto>> Attend(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;

            //var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            //string token = "";

            //if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            //{
            //    token = authorizationHeader.Substring("Bearer ".Length).Trim();
            //}

            //var userId = _jwtBearerService.GetSidFromToken(token);

            var jsonOptions = _cache.Get("fido2.attestationOptions").ToString();

            if (string.IsNullOrEmpty(jsonOptions))
            {
                return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.SessionEnded, HttpStatusCode.BadRequest);
            }

            var options = CredentialCreateOptions.FromJson(jsonOptions);
            var userId = BitConverter.ToInt64(options.User.Id);

            //if (userId == null)
            //{
            //    return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            //}

            // ToDo : vacations
            var day = now.DayOfWeek;
            var IsDayFoundAsVacation = await _unitOfWork
                    .Repository
                    .AnyAsync<Vacation>(
                        p => (p.Day == day || (
                            (p.AlternativeDate.HasValue && p.AlternativeDate.Value.Date == now.Date)
                            ||
                            (!p.AlternativeDate.HasValue && p.VacationDate.HasValue && p.VacationDate.Value.Date == now.Date)) && !p.IsDeleted && p.IsActive
                        ),
                        cancellationToken
                    );

            if (IsDayFoundAsVacation)
            {
                return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.DayIsVaction, HttpStatusCode.NotFound);
            }

            var attend = new EmployeeAttendance();
            attend.UserId = userId;
            attend.Date = DateOnly.FromDateTime(now.Date);
            attend.Time = now.TimeOfDay;

            // ToDo : attendance polices

            await _unitOfWork.Repository.AddAsync(attend, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            var responseDto = new AttendUserResponseDto(
                attend.Id,
                attend.UserId,
                attend.Date,
                attend.Time);

            return ApiResponse<AttendUserResponseDto>.ReturnSuccessResponse(responseDto, Messages.AttendanceRecordedSuccessfully);
        }

        public async Task<ApiResponse<CredentialCreateOptions>> AttendFidoUser(CancellationToken cancellationToken = default)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            string token = "";

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            var userId = _jwtBearerService.GetSidFromToken(token);

            if (userId == null)
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            var user = await _unitOfWork
                .Repository
                .GetQueryable<User>()
                .Where(u => u.Id == userId)
                .Select(u => new {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.DisplayName,
                    u.IsActive,
                    u.IsDeleted
                }).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            if (!user.IsActive || user.IsDeleted)
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.UserIsNotActive, HttpStatusCode.BadRequest);
            }

            //var existingDevice = await _unitOfWork
            //    .Repository
            //    .GetQueryable<UserDetails>()
            //    .AsNoTracking()
            //    .Where(u => u.UserId == userId && u.IsActive)
            //    .FirstOrDefaultAsync(cancellationToken);

            //if (existingDevice != null)
            //{
            //    return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.YouRegisteredYourDeviceBefore, HttpStatusCode.BadRequest);
            //}

            var fidoUser = new Fido2User
            {
                Id = BitConverter.GetBytes(user.Id),
                Name = user.UserName,
                DisplayName = user.DisplayName
            };

            // Platform Authenticator (Windows Hello / Touch ID)
            var authenticatorSelection = new AuthenticatorSelection
            {
                AuthenticatorAttachment = AuthenticatorAttachment.Platform, // platform only
                RequireResidentKey = true, // save it in pc
                UserVerification = UserVerificationRequirement.Required //Pin required
            };

            var exts = new AuthenticationExtensionsClientInputs
            {
                Extensions = true,
                UserVerificationMethod = true
            };

            var excludeCredentials = new List<PublicKeyCredentialDescriptor>();

            var options = _fido.RequestNewCredential(
                fidoUser,
                excludeCredentials,
                authenticatorSelection,
                AttestationConveyancePreference.None,
                exts
            );

            _cache.Set("fido2.attestationOptions", options.ToJson(), TimeSpan.FromMinutes(15));
            //HttpContext.Session.SetString("fido2.attestationOptions", options.ToJson()) for session

            return ApiResponse<CredentialCreateOptions>.ReturnSuccessResponse(options);
        }

        public async Task<ApiResponse<PaginatedList<GetAllChecksResponseDto>>> GetAllChecks(GetAllChecksRequestDto getAllChecksRequestDto, CancellationToken cancellationToken = default)
        {
            var data = _unitOfWork.Repository
               .GetQueryable<EmployeeAttendanceReport>()
               .Where(a => 
                    a.Date >= getAllChecksRequestDto.From && 
                    a.Date <= getAllChecksRequestDto.To)
               .Select(a => new GetAllChecksResponseDto(
                   a.EmployeeId,
                   a.EmployeeName,
                   a.DepartmentId,
                   a.DepartmentName,
                   a.Date,
                   a.CheckIn,
                   a.CheckOut == a.CheckIn ? null : a.CheckOut,
                   a.HoursWorked
               ));

            var response = await data.PaginatedListAsync(getAllChecksRequestDto.PageNumber, getAllChecksRequestDto.PageSize);

            return ApiResponse<PaginatedList<GetAllChecksResponseDto>>.ReturnSuccessResponse(response);
        }

        public async Task<ApiResponse<PaginatedList<GetEmployeeLatenessResponseDto>>> GetEmployeeLateness(GetGetEmployeeLatenessRequestDto getGetEmployeeLatenessRequestDto, CancellationToken cancellationToken = default)
        {
            var data = _unitOfWork.Repository
               .GetQueryable<EmployeeLatenessReport>()
               .Where(a =>
                    a.Date >= getGetEmployeeLatenessRequestDto.From &&
                    a.Date <= getGetEmployeeLatenessRequestDto.To)
               .Select(a => new GetEmployeeLatenessResponseDto(
                   a.EmployeeId,
                   a.EmployeeName,
                   a.DepartmentId,
                   a.DepartmentName,
                   a.AttendanceId,
                   a.Date,
                   a.CheckInTime,
                   a.PolicyId,
                   a.PolicyName,
                   a.PolicyAction,
                   a.ActionValue,
                   a.LateMinutes,
                   a.IsActionTaken,
                   a.AppliedHours,
                   a.ActionAppliedStatus,
                   a.ActionClarification
               ));

            var response = await data.PaginatedListAsync(getGetEmployeeLatenessRequestDto.PageNumber, getGetEmployeeLatenessRequestDto.PageSize);

            return ApiResponse<PaginatedList<GetEmployeeLatenessResponseDto>>.ReturnSuccessResponse(response);
        }
    }
}
