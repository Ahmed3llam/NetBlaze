using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class UserController : BaseNetBlazeController, IUserService
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("update-profile")]
        public async Task<ApiResponse<long>> UpdateUserDataAsync(UpdateUserDataRequestDto updateUserDataRequestDto, CancellationToken cancellationToken = default)
        {
            return await _userService.UpdateUserDataAsync(updateUserDataRequestDto, cancellationToken);
        }

        [HttpPost("update-role")]
        public async Task<ApiResponse<long>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserRoleRequestDto, CancellationToken cancellationToken = default)
        {
            return await _userService.UpdateUserRoleAsync(updateUserRoleRequestDto, cancellationToken);
        }
    }
}
