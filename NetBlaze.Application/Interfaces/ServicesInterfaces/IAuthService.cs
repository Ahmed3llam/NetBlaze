using Fido2NetLib;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterUserResponseDto>> Register(RegisterUserRequestDto registerUserRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<CredentialCreateOptions>> RegisterFidoUser(RegisterFidoUserRequestDto registerFidoUserRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<long>> RegisterUserCredential(AuthenticatorAttestationRawResponse attestation, CancellationToken cancellationToken = default);
    }
}
