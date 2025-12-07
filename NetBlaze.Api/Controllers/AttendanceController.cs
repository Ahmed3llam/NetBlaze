using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Attendance.Request;
using NetBlaze.SharedKernel.Dtos.Attendance.Response;
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
        [Authorize]
        public async Task<ApiResponse<AttendUserResponseDto>> Attend(AttendUserRequestDto attendUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _attendService.Attend(attendUserRequestDto, cancellationToken);
        }
    }
}
