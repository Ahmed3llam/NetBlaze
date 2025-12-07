using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtBearerService _jwtBearerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(
            IUnitOfWork unitOfWork,
            IJwtBearerService jwtBearerService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
        )
        {
            _unitOfWork = unitOfWork;
            _jwtBearerService = jwtBearerService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<long>> UpdateUserDataAsync(UpdateUserDataRequestDto updateUserDataRequestDto, CancellationToken cancellationToken = default)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            string token = "";

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                token = authorizationHeader.Substring("Bearer ".Length).Trim();


            var userId = _jwtBearerService.GetSidFromToken(token);

            var user = await _unitOfWork
                .Repository
                .GetSingleAsync<User>(
                    true,
                    p => p.Id == userId,
                    cancellationToken
                );

            if (user == null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);

            if (user.Email != updateUserDataRequestDto.Email)
            {
                var IsFoundUserWithSameEmail = await _unitOfWork
                    .Repository
                    .AnyAsync<User>(
                        p => p.Email == updateUserDataRequestDto.Email,
                        cancellationToken
                    );

                if (IsFoundUserWithSameEmail)
                    return ApiResponse<long>.ReturnFailureResponse(Messages.EmailAlreadyExists, HttpStatusCode.NotFound);
            }

            user.Email = updateUserDataRequestDto.Email;
            user.PhoneNumber = updateUserDataRequestDto.PhoneNumber;
            user.DisplayName = updateUserDataRequestDto.FullName;
            user.ManagerId = updateUserDataRequestDto.ManagerId;
            user.DepartmentId = updateUserDataRequestDto.DepartmentId;

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<long>.ReturnSuccessResponse(user.Id, Messages.UserDataUpdatedSuccessfully);
            }

            return ApiResponse<long>.ReturnSuccessResponse(user.Id, Messages.UserDataNotModified, HttpStatusCode.NotModified);
        }

        public async Task<ApiResponse<long>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserRoleRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork
                .Repository
                .GetSingleAsync<User>(
                    true,
                    p => p.Id == updateUserRoleRequestDto.UserId,
                    q => q.Include(u => u.UserRoles),
                    cancellationToken: cancellationToken
                );

            if (user == null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);

            var existingUserRole = user.UserRoles.FirstOrDefault();
            if (existingUserRole != null)
                _unitOfWork.Repository.HardDeleteAsync(existingUserRole);

            var role = await _roleManager.FindByIdAsync(updateUserRoleRequestDto.RoleId.ToString());

            if (role == null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.RoleNotFound, HttpStatusCode.BadRequest);

            await _userManager.AddToRoleAsync(user, role.Name);

            return ApiResponse<long>.ReturnSuccessResponse(user.Id, Messages.UserDataUpdatedSuccessfully);

            //var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            //if (rowsAffected > 0)
            //{
            //    return ApiResponse<long>.ReturnSuccessResponse(user.Id, Messages.UserDataUpdatedSuccessfully);
            //}

            //return ApiResponse<long>.ReturnSuccessResponse(user.Id, Messages.UserDataNotModified, HttpStatusCode.NotModified);
        }
    }
}
