using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Attendance.Request;
using NetBlaze.SharedKernel.Dtos.Attendance.Response;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class AttendanceController : BaseNetBlazeController, IAttendService
    {
        private readonly IAttendService _attendService;

        public AttendanceController(IAttendService attendService)
        {
            _attendService = attendService;
        }

        [HttpPost("attend")]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<AttendUserResponseDto>> Attend(CancellationToken cancellationToken = default)
        {
            return await _attendService.Attend(cancellationToken);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<PaginatedList<GetAllChecksResponseDto>>> GetAllChecks([FromQuery] GetAllChecksRequestDto getAllChecksRequestDto, CancellationToken cancellationToken = default)
        {
            return await _attendService.GetAllChecks(getAllChecksRequestDto, cancellationToken);
        }

        [HttpGet("employees-lateness")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<PaginatedList<GetEmployeeLatenessResponseDto>>> GetEmployeeLateness([FromQuery] GetGetEmployeeLatenessRequestDto getGetEmployeeLatenessRequestDto, CancellationToken cancellationToken = default)
        {
            return await _attendService.GetEmployeeLateness(getGetEmployeeLatenessRequestDto, cancellationToken);
        }
    }
}
