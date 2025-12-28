using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Net.Http.Json;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazUserService : BaseBlazService
    {
        public BlazUserService(ExternalHttpClientWrapper externalHttpClientWrapper, CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<BaseResponseDto>>> GetListedManagersAsync(CancellationToken cancellationToken = default)
        {
            var response = await _externalHttpClientWrapper.NativeHttpClient.GetAsync(ApiRelativePaths.USER_LIST_MANAGERS, cancellationToken);

            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BaseResponseDto>>>(cancellationToken: cancellationToken);

            return apiResponse;
        }
    }
}
