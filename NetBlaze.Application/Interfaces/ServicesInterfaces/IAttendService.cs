using Fido2NetLib;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Attendance.Request;
using NetBlaze.SharedKernel.Dtos.Attendance.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAttendService
    {
        Task<ApiResponse<CredentialCreateOptions>> AttendFidoUser(CancellationToken cancellationToken = default);
        Task<ApiResponse<AttendUserResponseDto>> Attend(AuthenticatorAttestationRawResponse attestationm, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetAllChecksResponseDto>>> GetAllChecks(GetAllChecksRequestDto getAllChecksRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<PaginatedList<GetEmployeeLatenessResponseDto>>> GetEmployeeLateness(GetGetEmployeeLatenessRequestDto getGetEmployeeLatenessRequestDto, CancellationToken cancellationToken = default);
    }
}
