using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Response;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    public class RandomCheckController : BaseNetBlazeController, IRandomCheckService
    {
        private readonly IRandomCheckService _randomCheckService;

        public RandomCheckController(IRandomCheckService randomCheckService)
        {
            _randomCheckService = randomCheckService;
        }

        [HttpPost("generate-otp")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ApiResponse<object>> GenerateOtpAsync(CancellationToken cancellationToken = default)
        {
            return await _randomCheckService.GenerateOtpAsync(cancellationToken);
        }

        [HttpPut("reply")]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<bool>> RandomCheckReplyAsync(RandomCheckRequestReplyDto randomCheckRequestReply, CancellationToken cancellationToken = default)
        {
            return await _randomCheckService.RandomCheckReplyAsync(randomCheckRequestReply, cancellationToken);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<PaginatedList<GetAllRandomChecksForUserResponseDto>>> GetAllRandomChecksForUser([FromQuery] GetAllRandomChecksForUserRequestDto getAllRandomChecksForUserRequestDto, CancellationToken cancellationToken = default)
        {
            return await _randomCheckService.GetAllRandomChecksForUser(getAllRandomChecksForUserRequestDto, cancellationToken);
        }

    }
}
