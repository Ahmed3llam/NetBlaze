using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class VacationController : BaseNetBlazeController, IVacationService
    {
        private readonly IVacationService _vacationService;

        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpGet("list")]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public Task<ApiResponse<PaginatedList<GetListedVacationResponseDto>>> GetListedVacations([FromQuery] PaginateRequestDto paginateRequestDto)
        {
            return _vacationService.GetListedVacations(paginateRequestDto);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return _vacationService.GetVacationByIdAsync(id, cancellationToken);
        }

        [HttpPost("add")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ApiResponse<object>> AddVacationAsync(AddVacationRequestDto addVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await _vacationService.AddVacationAsync(addVacationRequestDto, cancellationToken);
        }

        [HttpPut("update")]
        [Authorize(Roles = nameof(Role.Admin))]
        public Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return _vacationService.UpdateVacationAsync(updateVacationRequestDto, cancellationToken);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.DeleteVacationAsync(id, cancellationToken);
        }
    }
}
