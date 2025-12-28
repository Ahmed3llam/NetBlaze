using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedPolicesAsync(PaginateRequestDto paginateRequestDto)
        {
            var listedRoles = _unitOfWork
                .Repository
                .GetQueryable<Role>()
                .Select(p => new BaseResponseDto(
                        p.Id,
                        p.Name,
                        p.IsActive));

            var result = await PaginatedList<BaseResponseDto>.CreateAsync(listedRoles, paginateRequestDto.PageNumber, paginateRequestDto.PageSize);

            return ApiResponse<PaginatedList<BaseResponseDto>>.ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<List<BaseResponseDto>>> GetListedRolesAsync()
        {
            var listedRoles = await _unitOfWork
                .Repository
                .GetMultipleAsync<Role, BaseResponseDto>(
                    true,
                    x => x.IsActive,
                    x => new BaseResponseDto(x.Id, x.Name, x.IsActive)
                );

            return ApiResponse<List<BaseResponseDto>>.ReturnSuccessResponse(listedRoles);
        }
    }
}
