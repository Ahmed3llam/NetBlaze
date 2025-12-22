using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class RoleController : BaseNetBlazeController, IRoleService
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("list")]
        public IAsyncEnumerable<BaseResponseDto> GetListedRoles()
        {
            return _roleService.GetListedRoles();
        }

        [HttpGet("paginate")]
        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedPolices(PaginateRequestDto paginateRequestDto)
        {
            return await _roleService.GetPaginatedPolices(paginateRequestDto);
        }
    }
}
