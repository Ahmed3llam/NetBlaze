using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedPolicesAsync(PaginateRequestDto paginateRequestDto);
        Task<ApiResponse<List<BaseResponseDto>>> GetListedRolesAsync();
    }
}
