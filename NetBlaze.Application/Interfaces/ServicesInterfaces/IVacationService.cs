using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IVacationService
    {
        IAsyncEnumerable<GetListedVacationResponseDto> GetListedVacations();

        Task<ApiResponse<GetVacationResponseDto>> GetVacationAsync(long id, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> AddVacationAsync(AddVacationRequestDto addVacationRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default);
    }
}
