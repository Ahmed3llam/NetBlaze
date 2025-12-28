using Blazored.LocalStorage;
using Fido2NetLib;
using Microsoft.AspNetCore.Components;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Pages;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazAuthService : BaseBlazService
    {
        private readonly GlobalUserContext _globalUserContext;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private const string token = nameof(token);
        public BlazAuthService(
            ExternalHttpClientWrapper externalHttpClientWrapper,
            CentralizedSnackbarProvider centralizedSnackbarProvider,
            GlobalUserContext globalUserContext,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager)
            : base(externalHttpClientWrapper, centralizedSnackbarProvider) 
        {
            _globalUserContext = globalUserContext;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }

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

        public async Task HandleAuthenticationProcessForUserAsync()
        {
            var _token = await GetDecryptedTokenFromLocalStorageAsync();

            if (string.IsNullOrWhiteSpace(_token))
            {
                _navigationManager.NavigateTo(nameof(LoginPage));
                return;
            }

            var validationResult = await ValidateTokenInBackEndAsync(_token);

            if (validationResult.IsValid)
            {
                var decodedToken = DecodeToken(_token);

                FillGlobalUserContextFromToken(decodedToken);
            }
            else
            {
                EmptyGlobalUserContext();

                _navigationManager.NavigateTo(nameof(LoginPage));
            }
        }

        private async Task<GetTokenValidationResultResponseDto> ValidateTokenInBackEndAsync(string token, CancellationToken cancellationToken = default)
        {
            var response = await _externalHttpClientWrapper.NativeHttpClient.GetAsync(ApiRelativePaths.USER_VALIDATE_TOKEN + token, cancellationToken);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GetTokenValidationResultResponseDto>>(cancellationToken: cancellationToken);

            return apiResponse.Data;
        }

        public async Task<string> GetDecryptedTokenFromLocalStorageAsync()
        {
            return await _localStorageService.GetItemAsStringAsync(token) ?? string.Empty;
        }

        public async Task SaveEncryptedTokenToLocalStorageAsync(string encryptedToken)
        {
            await _localStorageService.SetItemAsStringAsync(token, encryptedToken);
        }

        public JwtSecurityToken DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(token))
                return tokenHandler.ReadJwtToken(token);

            return null;
        }

        public async Task<bool> IsTokenExpiredOrRemovedAsync()
        {
            var jwt = await GetDecryptedTokenFromLocalStorageAsync();

            if (!string.IsNullOrWhiteSpace(jwt))
            {
                var decodedToken = DecodeToken(jwt);

                var currentTime = DateTime.UtcNow;

                return decodedToken!.ValidTo <= currentTime;
            }

            return true;
        }
        private void FillGlobalUserContextFromToken(JwtSecurityToken jwtSecurityToken)
        {
            if (jwtSecurityToken is not null)
            {
                var userIdClaimValue = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid)?.Value;
                _globalUserContext.UserId = long.TryParse(userIdClaimValue, out long parsedUserId) ? parsedUserId : 0;

                var emailClaimValue = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
                _globalUserContext.Email = emailClaimValue;

                var UserNameClaimValue = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                _globalUserContext.UserName = UserNameClaimValue;
            }
        }

        private void EmptyGlobalUserContext()
        {
            _globalUserContext.UserId = 0;
            _globalUserContext.Email = string.Empty;
            _globalUserContext.UserName = string.Empty;
        }
    }
}
