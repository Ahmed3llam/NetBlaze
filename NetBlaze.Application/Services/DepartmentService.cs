using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PaginatedList<BaseResponseDto>>> GetPaginatedDepartmentsAsync(PaginateRequestDto paginateRequestDto)
        {
            var listedDepartments = _unitOfWork
                .Repository
                .GetQueryable<Department>()
                .Select(p => new BaseResponseDto(
                        p.Id,
                        p.Name,
                        p.IsActive));

            var result = await PaginatedList<BaseResponseDto>.CreateAsync(listedDepartments, paginateRequestDto.PageNumber, paginateRequestDto.PageSize);

            return ApiResponse<PaginatedList<BaseResponseDto>>.ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<List<BaseResponseDto>>> GetListedDepartmentsAsync()
        {
            var listedDepartments = await _unitOfWork
                .Repository
                .GetMultipleAsync<Department, BaseResponseDto>(
                    true,
                    x => x.IsActive,
                    x => new BaseResponseDto(x.Id, x.Name, x.IsActive)
                );

            return ApiResponse<List<BaseResponseDto>>.ReturnSuccessResponse(listedDepartments);
        }
    }
}
