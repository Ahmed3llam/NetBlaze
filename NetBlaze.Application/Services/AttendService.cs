using Microsoft.AspNetCore.Http;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
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
            var now = DateTime.UtcNow;

            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            string token = "";

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var userId = _jwtBearerService.GetSidFromToken(token);

            if (userId == null)
                return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);

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
                return ApiResponse<AttendUserResponseDto>.ReturnFailureResponse(Messages.DayIsVaction, HttpStatusCode.NotFound);

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
    }
}
