using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Policy.Request;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IPolicyService
    {
        Task<ApiResponse<PaginatedList<GetListedPolicyResponseDto>>> GetListedPolices(PaginateRequestDto paginateRequestDto);

        Task<ApiResponse<GetPolicyResponseDto>> GetPolicyByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default);
    }
}
