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
        public async Task<ApiResponse<List<BaseResponseDto>>> GetListedDepartmentsAsync()
        {
            return await _departmentService.GetListedDepartmentsAsync();
        }

        [HttpGet("paginate")]
        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedDepartmentsAsync(PaginateRequestDto paginateRequestDto)
        {
            return await _departmentService.GetPaginatedDepartmentsAsync(paginateRequestDto);
        }
    }
}
