using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazDepartmentService : BaseBlazService
    {
        public BlazDepartmentService(ExternalHttpClientWrapper externalHttpClientWrapper, CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<List<BaseResponseDto>>> GetListedDepartments([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await _externalHttpClientWrapper.NativeHttpClient.GetAsync(ApiRelativePaths.DEPARTMENT_LIST, cancellationToken);

            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BaseResponseDto>>>(cancellationToken: cancellationToken);

            return apiResponse;
        }
    }
}
