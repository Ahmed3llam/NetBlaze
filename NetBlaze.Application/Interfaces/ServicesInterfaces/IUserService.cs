using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.ServicesInterfaces
{
    public interface IUserService
    {
        Task<ApiResponse<long>> UpdateUserDataAsync(UpdateUserDataRequestDto updateUserDataRequestDto, CancellationToken cancellationToken = default);

        Task<ApiResponse<long>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserRoleRequestDto, CancellationToken cancellationToken = default);
    }
}
