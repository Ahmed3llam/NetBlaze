using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedDepartments(PaginateRequestDto paginateRequestDto);
        IAsyncEnumerable<BaseResponseDto> GetListedDepartments();
    }
}
