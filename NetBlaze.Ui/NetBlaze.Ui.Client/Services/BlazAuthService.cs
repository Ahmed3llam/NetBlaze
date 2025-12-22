using Fido2NetLib;
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

        public async Task<ApiResponse<CredentialCreateOptions>> RegisterFidoStart(RegisterFidoUserRequestDto registerFidoUserRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<RegisterFidoUserRequestDto, ApiResponse<CredentialCreateOptions>>(ApiRelativePaths.AUTH_REGISTER_FIDO_START, registerFidoUserRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<long>> RegisterFidoComplete(AuthenticatorAttestationRawResponse attestationRawResponse, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<AuthenticatorAttestationRawResponse, ApiResponse<long>>(ApiRelativePaths.AUTH_REGISTER_FIDO_COMPLETE, attestationRawResponse, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _externalHttpClientWrapper.PostAsJsonAsync<LoginUserRequestDto, ApiResponse<LoginUserResponseDto>>(ApiRelativePaths.AUTH_LOGIN, loginUserRequestDto, cancellationToken);

            _centralizedSnackbarProvider.ShowApiResponseSnackbar(apiResponse);

            return apiResponse;
        }
    }
}
