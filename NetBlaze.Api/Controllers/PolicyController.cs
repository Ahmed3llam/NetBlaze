using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Policy.Request;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize]
    public class PolicyController : BaseNetBlazeController, IPolicyService
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet("list")]
        public IAsyncEnumerable<GetListedPolicyResponseDto> GetListedPolices()
        {
            return _policyService.GetListedPolices();
        }

        public Task<ApiResponse<GetPolicyResponseDto>> GetPolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            return _policyService.GetPolicyAsync(id, cancellationToken);
        }

        [HttpPost("add")]
        public async Task<ApiResponse<object>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return await _policyService.AddPolicyAsync(addPolicyRequestDto, cancellationToken);
        }

        [HttpPost("update")]
        public Task<ApiResponse<object>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return _policyService.UpdatePolicyAsync(updatePolicyRequestDto, cancellationToken);
        }

        [HttpPost("delete")]
        public async Task<ApiResponse<object>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.DeletePolicyAsync(id, cancellationToken);
        }
    }
}
