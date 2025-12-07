using NetBlaze.SharedKernel.Dtos.Policy.Request;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IPolicyService
    {
        IAsyncEnumerable<GetListedPolicyResponseDto> GetListedPolices();

        Task<ApiResponse<GetPolicyResponseDto>> GetPolicyAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default);
    }
}
