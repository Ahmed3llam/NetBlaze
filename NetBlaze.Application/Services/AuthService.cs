using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.Dtos.User.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;
using System.Security.Claims;

namespace NetBlaze.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public readonly IJwtBearerService _jwtBearerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Fido2 _fido;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IJwtBearerService jwtBearerService,
            IUnitOfWork unitOfWork,
            Fido2 fido,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtBearerService = jwtBearerService;
            _unitOfWork = unitOfWork;
            _fido = fido;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<RegisterUserResponseDto>> Register(RegisterUserRequestDto registerUserRequestDto, CancellationToken cancellationToken = default)
        {
            var existingEmail = await _userManager.FindByEmailAsync(registerUserRequestDto.Email);

            if (existingEmail != null)
            {
                return ApiResponse<RegisterUserResponseDto>.ReturnFailureResponse(Messages.EmailAlreadyExists, HttpStatusCode.BadRequest);
            }

            User newUser = new User()
            {
                DisplayName = registerUserRequestDto.FullName,
                Email = registerUserRequestDto.Email,
                PhoneNumber = registerUserRequestDto.PhoneNumber,
                UserName = registerUserRequestDto.Email,
                ManagerId = registerUserRequestDto.ManagerId,
                DepartmentId = registerUserRequestDto.DepartmentId
            };

            var result = await _userManager.CreateAsync(newUser, registerUserRequestDto.Password);

            if (!result.Succeeded)
            {
                return ApiResponse<RegisterUserResponseDto>.ReturnFailureResponse(Messages.RegistrationFailed, HttpStatusCode.NotFound);
            }

            var role = await _roleManager.FindByIdAsync(registerUserRequestDto.RoleId.ToString());

            if (role == null)
            {
                return ApiResponse<RegisterUserResponseDto>.ReturnFailureResponse(Messages.RoleNotFound, HttpStatusCode.BadRequest);
            }

            await _userManager.AddToRoleAsync(newUser, role.Name);

            var response = new RegisterUserResponseDto(
                newUser.Id);

            return ApiResponse<RegisterUserResponseDto>.ReturnSuccessResponse(response, Messages.RegistrationDoneSuccessfully);
        }

        public async Task<ApiResponse<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDto.Email);

            if (user == null)
            {
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            if (!user.IsActive || user.IsDeleted)
            {
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.UserIsNotActive, HttpStatusCode.BadRequest);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserRequestDto.Password);

            if (!isPasswordValid)
            {
                return ApiResponse<LoginUserResponseDto>.ReturnFailureResponse(Messages.InCorrectPassword, HttpStatusCode.BadRequest);
            }

            var roles = await _userManager.GetRolesAsync(user);

            GenerateTokenRequestDto tokenRequestDto = new GenerateTokenRequestDto(
                user.Id, 
                user.UserName, 
                user.Email,
                roles?.ToList());

            var token = _jwtBearerService.GenerateToken(tokenRequestDto);

            var response = new LoginUserResponseDto(
                user.Id, 
                user.DisplayName, 
                user.UserName, 
                token, 
                user.Email, 
                user.PhoneNumber);

            return ApiResponse<LoginUserResponseDto>.ReturnSuccessResponse(response, Messages.LoginSuccessfully);
        }

        public async Task<ApiResponse<CredentialCreateOptions>> RegisterFidoUser(RegisterFidoUserRequestDto registerFidoUserRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork
                .Repository
                .GetQueryable<User>()
                .Where(u => u.Id == registerFidoUserRequestDto.UserId)
                .Select(u => new {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.DisplayName,
                    u.IsActive,
                    u.IsDeleted
                }).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            if (!user.IsActive || user.IsDeleted)
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.UserIsNotActive, HttpStatusCode.BadRequest);
            }

            var existingDevice = await _unitOfWork
                .Repository
                .GetQueryable<UserDetails>()
                .AsNoTracking()
                .Where(u => u.UserId == registerFidoUserRequestDto.UserId && u.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingDevice != null) 
            {
                return ApiResponse<CredentialCreateOptions>.ReturnFailureResponse(Messages.YouRegisteredYourDeviceBefore, HttpStatusCode.BadRequest);
            }

            var fidoUser = new Fido2User
            {
                Id = BitConverter.GetBytes(user.Id),
                Name = user.UserName,
                DisplayName = user.DisplayName
            };

            // Platform Authenticator (Windows Hello / Touch ID)
            var authenticatorSelection = new AuthenticatorSelection
            {
                AuthenticatorAttachment = AuthenticatorAttachment.Platform, // platform only
                RequireResidentKey = true, // save it in pc
                UserVerification = UserVerificationRequirement.Required //Pin required
            };

            var exts = new AuthenticationExtensionsClientInputs
            {
                Extensions = true,
                UserVerificationMethod = true
            };

            var excludeCredentials = new List<PublicKeyCredentialDescriptor>();

            var options = _fido.RequestNewCredential(
                fidoUser,
                excludeCredentials,
                authenticatorSelection,
                AttestationConveyancePreference.None,
                exts
            );

            _cache.Set("fido2.attestationOptions", options.ToJson(), TimeSpan.FromMinutes(15));
            //HttpContext.Session.SetString("fido2.attestationOptions", options.ToJson()) for session

            return ApiResponse<CredentialCreateOptions>.ReturnSuccessResponse(options);
        }

        public async Task<ApiResponse<long>> RegisterUserCredential(AuthenticatorAttestationRawResponse attestation, CancellationToken cancellationToken = default)
        {
            //var jsonOptions = HttpContext.Session.GetString("fido2.attestationOptions"); for session
            var jsonOptions = _cache.Get("fido2.attestationOptions").ToString();

            if (string.IsNullOrEmpty(jsonOptions))
            {
                return ApiResponse<long>.ReturnFailureResponse(Messages.SessionEnded, HttpStatusCode.BadRequest);
            }

            var options = CredentialCreateOptions.FromJson(jsonOptions);
            var userId = BitConverter.ToInt64(options.User.Id);

            var existingDevice = await _unitOfWork
                .Repository
                .GetQueryable<UserDetails>()
                .AsNoTracking()
                .Where(u => u.UserId == userId && u.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingDevice != null)
            {
                return ApiResponse<long>.ReturnFailureResponse(Messages.YouRegisteredYourDeviceBefore, HttpStatusCode.BadRequest);
            }

            var success = await _fido.MakeNewCredentialAsync(
                attestation,
                options,
                async (args, cancellationToken) =>
                {
                    // validate this credentials not in other devices
                    var creds = await _unitOfWork
                        .Repository
                        .AnyAsync<UserDetails>(c => c.CredentialId == args.CredentialId, cancellationToken);
                    return !creds;
                }
            );

            // collect device info
            var userAgent = _httpContextAccessor.HttpContext.Request.Headers.UserAgent.ToString();
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            var acceptLanguage = _httpContextAccessor.HttpContext.Request.Headers.AcceptLanguage.ToString();

            // Device Fingerprint 
            var deviceFingerprint = $"{userAgent}|{acceptLanguage}";

            var credential = new UserDetails
            {
                UserId = userId,
                CredentialId = success.Result!.CredentialId,
                PublicKey = success.Result.PublicKey,
                SignatureCounter = success.Result.Counter,
                CredType = success.Result.CredType,
                AaGuid = success.Result.Aaguid,
                DeviceInfo = deviceFingerprint,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                IsActive = true
            };

            await _unitOfWork.Repository.AddAsync(credential, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<long>.ReturnSuccessResponse(userId, Messages.DeviceHasBeenSuccessfullyRegistered);
        }
    }
}
