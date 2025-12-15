using Fido2NetLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseNetBlazeController, IAuthService
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _authService.Login(loginUserRequestDto, cancellationToken);
        }

        [HttpPost("register")]
        public async Task<ApiResponse<RegisterUserResponseDto>> Register(RegisterUserRequestDto registerUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _authService.Register(registerUserRequestDto, cancellationToken);
        }

        [HttpPost("register-fido-user")]
        public async Task<ApiResponse<CredentialCreateOptions>> RegisterFidoUser(RegisterFidoUserRequestDto registerFidoUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _authService.RegisterFidoUser(registerFidoUserRequestDto, cancellationToken);
        }

        [HttpPost("register-user-credential")]
        public async Task<ApiResponse<long>> RegisterUserCredential(AuthenticatorAttestationRawResponse attestation, CancellationToken cancellationToken = default)
        {
            return await _authService.RegisterUserCredential(attestation, cancellationToken);
        }
    }
}
