using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Enums;
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

        [HttpGet("managers")]
        public async Task<ApiResponse<List<BaseResponseDto>>> GetAllManagersAsync(CancellationToken cancellationToken = default)
        {
            return await _userService.GetAllManagersAsync(cancellationToken);
        }

        [HttpPut("update-profile")]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<long>> UpdateUserDataAsync(UpdateUserDataRequestDto updateUserDataRequestDto, CancellationToken cancellationToken = default)
        {
            return await _userService.UpdateUserDataAsync(updateUserDataRequestDto, cancellationToken);
        }

        [HttpPut("update-role")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ApiResponse<long>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserRoleRequestDto, CancellationToken cancellationToken = default)
        {
            return await _userService.UpdateUserRoleAsync(updateUserRoleRequestDto, cancellationToken);
        }

        [HttpGet("validate-token")]
        public Task<ApiResponse<GetTokenValidationResultResponseDto>> ValidateTokenForUserAsync(string? bearerToken)
        {
            return _userService.ValidateTokenForUserAsync(bearerToken);
        }
    }
}
