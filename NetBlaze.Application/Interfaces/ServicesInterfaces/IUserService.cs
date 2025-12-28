using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<BaseResponseDto>>> GetAllManagersAsync(CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> UpdateUserDataAsync(UpdateUserDataRequestDto updateUserDataRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserRoleRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<GetTokenValidationResultResponseDto>> ValidateTokenForUserAsync(string? bearerToken);
    }
}
