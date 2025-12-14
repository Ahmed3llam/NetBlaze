using Microsoft.AspNetCore.Http;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
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

        public AttendService(
            IUnitOfWork unitOfWork,
            IJwtBearerService jwtBearerService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _jwtBearerService = jwtBearerService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<AttendUserResponseDto>> Attend(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;

            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            string token = "";

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            var userId = _jwtBearerService.GetSidFromToken(token);

            if (userId == null)
            {
                return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

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
            attend.UserId = userId.Value;
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
