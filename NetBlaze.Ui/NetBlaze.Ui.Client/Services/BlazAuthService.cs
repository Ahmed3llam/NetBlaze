using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazAuthService : BaseBlazService
    {
        public BlazAuthService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async Task<ApiResponse<RegisterUserResponseDto>> Register(RegisterUserRequestDto registerUserRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<RegisterUserRequestDto, ApiResponse<RegisterUserResponseDto>>(ApiRelativePaths.AUTH_REGISTER, registerUserRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
