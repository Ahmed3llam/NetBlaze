using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRandomCheckService
    {
        Task<ApiResponse<object>> GenerateOtpAsync(CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> RandomCheckReplyAsync(RandomCheckRequestReplyDto randomCheckRequestReply, CancellationToken cancellationToken = default);

        Task<ApiResponse<PaginatedList<GetAllRandomChecksForUserResponseDto>>> GetAllRandomChecksForUser(GetAllRandomChecksForUserRequestDto getAllRandomChecksForUserRequestDto, CancellationToken cancellationToken = default);
    }
}
