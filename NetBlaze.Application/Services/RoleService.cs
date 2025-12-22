using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
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

        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedPolices(PaginateRequestDto paginateRequestDto)
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

        public async IAsyncEnumerable<BaseResponseDto> GetListedRoles()
        {
            var listedRoles = _unitOfWork
                .Repository
                .GetMultipleStream<Role, BaseResponseDto>(
                    true,
                    x => x.IsActive,
                    x => new BaseResponseDto(x.Id, x.Name, x.IsActive)
                );

            await foreach (var role in listedRoles)
            {
                yield return role;
            }
        }
    }
}
