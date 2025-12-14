using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Policy.Request;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using System;

namespace NetBlaze.Api.Controllers
{
    public class PolicyController : BaseNetBlazeController, IPolicyService
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet("list")]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public Task<ApiResponse<PaginatedList<GetListedPolicyResponseDto>>> GetListedPolices([FromQuery] PaginateRequestDto paginateRequestDto)
        {
            return _policyService.GetListedPolices(paginateRequestDto);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Employee)},{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public Task<ApiResponse<GetPolicyResponseDto>> GetPolicyByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return _policyService.GetPolicyByIdAsync(id, cancellationToken);
        }

        [HttpPost("add")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]

        public async Task<ApiResponse<long>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return await _policyService.AddPolicyAsync(addPolicyRequestDto, cancellationToken);
        }

        [HttpPut("update")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public Task<ApiResponse<long>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return _policyService.UpdatePolicyAsync(updatePolicyRequestDto, cancellationToken);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ApiResponse<long>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _policyService.DeletePolicyAsync(id, cancellationToken);
        }

        [HttpPost("apply-policy")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<ApiResponse<bool>> ApplyPolicyAsync(ApplyPolicyRequestDto applyPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            return await _policyService.ApplyPolicyAsync(applyPolicyRequestDto, cancellationToken);
        }
    }
}
