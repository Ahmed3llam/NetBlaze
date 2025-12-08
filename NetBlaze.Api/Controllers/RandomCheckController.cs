using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
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
        public async Task<ApiResponse<object>> GenerateOtpAsync(CancellationToken cancellationToken = default)
        {
            return await _randomCheckService.GenerateOtpAsync(cancellationToken);
        }

        [HttpPut("reply")]
        public async Task<ApiResponse<bool>> RandomCheckReplyAsync(RandomCheckRequestReplyDto randomCheckRequestReply, CancellationToken cancellationToken = default)
        {
            return await _randomCheckService.RandomCheckReplyAsync(randomCheckRequestReply, cancellationToken);
        }
    }
}
