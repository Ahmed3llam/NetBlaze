using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazVacationService : BaseBlazService
    {
        public BlazVacationService(
            ExternalHttpClientWrapper externalHttpClientWrapper, 
            CentralizedSnackbarProvider centralizedSnackbarProvider) 
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<PaginatedList<GetListedVacationResponseDto>>> GetListedVacationsAsync(PaginateRequestDto paginateRequestDto, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var queryString = $"?{nameof(paginateRequestDto.PageNumber)}={paginateRequestDto.PageNumber}&" +
                $"{nameof(paginateRequestDto.PageSize)}={paginateRequestDto.PageSize}";

            var response = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<PaginatedList<GetListedVacationResponseDto>>>(
                ApiRelativePaths.VACATIONS_LIST + queryString,
                cancellationToken);

            //response.EnsureSuccessStatusCode();

            //var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PaginatedList<GetListedVacationResponseDto>>>(cancellationToken: cancellationToken);

            return response;
        }

        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.GetFromJsonAsync<ApiResponse<GetVacationResponseDto>>(
                $"{ApiRelativePaths.SAMPLE_GET}?{nameof(id)}={id}", 
                cancellationToken);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> AddVacationAsync(AddVacationRequestDto addVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<AddVacationRequestDto, ApiResponse<object>>(
                ApiRelativePaths.VACATIONS_ADD, addVacationRequestDto, 
                cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PutAsJsonAsync<UpdateVacationRequestDto, ApiResponse<object>>(
                ApiRelativePaths.VACATIONS_UPDATE, updateVacationRequestDto, 
                cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.DeleteFromJsonAsync<ApiResponse<object>>($"{
                ApiRelativePaths.VACATIONS_DELETE}?{nameof(id)}={id}", 
                cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
