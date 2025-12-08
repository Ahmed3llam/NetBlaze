using NetBlaze.SharedKernel.Dtos.Attendance.Request;
using NetBlaze.SharedKernel.Dtos.Attendance.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAttendService
    {
        Task<ApiResponse<AttendUserResponseDto>> Attend(CancellationToken cancellationToken = default);
    }
}
