using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize]
    public class VacationController : BaseNetBlazeController, IVacationService
    {
        private readonly IVacationService _vacationService;

        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpGet("list")]
        public IAsyncEnumerable<GetListedVacationResponseDto> GetListedVacations()
        {
            return _vacationService.GetListedVacations();
        }

        public Task<ApiResponse<GetVacationResponseDto>> GetVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            return _vacationService.GetVacationAsync(id, cancellationToken);
        }

        [HttpPost("add")]
        public async Task<ApiResponse<object>> AddVacationAsync(AddVacationRequestDto addVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await _vacationService.AddVacationAsync(addVacationRequestDto, cancellationToken);
        }

        [HttpPost("update")]
        public Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return _vacationService.UpdateVacationAsync(updateVacationRequestDto, cancellationToken);
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.DeleteVacationAsync(id, cancellationToken);
        }
    }
}
