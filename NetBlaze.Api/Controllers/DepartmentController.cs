using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class DepartmentController : BaseNetBlazeController, IDepartmentService
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("list")]
        public IAsyncEnumerable<BaseResponseDto> GetListedDepartments()
        {
            return _departmentService.GetListedDepartments();
        }

        [HttpGet("paginate")]
        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedDepartments(PaginateRequestDto paginateRequestDto)
        {
            return await _departmentService.GetPaginatedDepartments(paginateRequestDto);
        }
    }
}
