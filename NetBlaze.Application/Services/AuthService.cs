using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public readonly IJwtBearerService _jwtBearerService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IJwtBearerService jwtBearerService,
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtBearerService = jwtBearerService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<long>> Register(RegisterUserRequestDto registerUserRequestDto, CancellationToken cancellationToken = default)
        {
            var existingEmail = await _userManager.FindByEmailAsync(registerUserRequestDto.Email);

            if (existingEmail != null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.EmailAlreadyExists, HttpStatusCode.BadRequest);

            var existingPhone = await _userManager.Users.AnyAsync(x => x.PhoneNumber == registerUserRequestDto.PhoneNumber);

            if (existingPhone)
                return ApiResponse<long>.ReturnFailureResponse(Messages.PhoneAlreadyExists, HttpStatusCode.BadRequest);

            var departmentFound = await _unitOfWork
                .Repository.AnyAsync<Department>(d => d.Id == registerUserRequestDto.DepartmentId);

            if (!departmentFound)
                return ApiResponse<long>.ReturnFailureResponse(Messages.DepartmentNotFound, HttpStatusCode.BadRequest);

            if (registerUserRequestDto.ManagerId > 0)
            {
                var managerFound = await _userManager
                    .Users
                    .AnyAsync(x => x.Id == registerUserRequestDto.ManagerId, cancellationToken);

                if (!managerFound)
                    return ApiResponse<long>.ReturnFailureResponse(Messages.ManagerNotFound, HttpStatusCode.BadRequest);
            }

            User newUser = new User()
            {
                //DisplayName = registerUserRequestDto.FullName,
                Email = registerUserRequestDto.Email,
                PhoneNumber = registerUserRequestDto.PhoneNumber,
                UserName = registerUserRequestDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerUserRequestDto.Password);

            if (!result.Succeeded)
                return ApiResponse<long>.ReturnFailureResponse(Messages.RegistrationFailed, HttpStatusCode.NotFound);

            var role = await _roleManager.FindByIdAsync(registerUserRequestDto.RoleId.ToString());

            if (role == null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.RoleNotFound, HttpStatusCode.BadRequest);

            await _userManager.AddToRoleAsync(newUser, role.Name);

            return ApiResponse<long>.ReturnSuccessResponse(newUser.Id, Messages.RegistrationDoneSuccessfully);
        }

        public async Task<ApiResponse<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDto.Email);

            if (user == null)
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);

            if (!user.IsActive || user.IsDeleted)
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.UserIsNotActive, HttpStatusCode.BadRequest);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserRequestDto.Password);

            if (!isPasswordValid)
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.InCorrectPassword, HttpStatusCode.BadRequest);

            var roles = await _userManager.GetRolesAsync(user);

            GenerateTokenRequestDto tokenRequestDto = new GenerateTokenRequestDto(
                user.Id, 
                user.UserName, 
                user.Email,
                roles.ToList());

            var token = _jwtBearerService.GenerateToken(tokenRequestDto);

            var response = new LoginUserResponseDto(
                user.Id, 
                user.DisplayName, 
                user.UserName, 
                token, 
                user.Email, 
                user.PhoneNumber);

            await _signInManager.SignInAsync(user, loginUserRequestDto.RememberMe);

            return ApiResponse<LoginUserResponseDto>.ReturnSuccessResponse(response, Messages.LoginSuccessfully);
        }
    }
}
