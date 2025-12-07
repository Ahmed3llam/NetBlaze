using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Policy.Request;
using NetBlaze.SharedKernel.Dtos.Policy.Response;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PolicyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async IAsyncEnumerable<GetListedPolicyResponseDto> GetListedPolices()
        {
            var listedPolicies = _unitOfWork
                .Repository
                .GetMultipleStream<Policy, GetListedPolicyResponseDto>(
                    true,
                    _ => true,
                    p => new GetListedPolicyResponseDto(
                        p.Id, 
                        p.PolicyName, 
                        p.WorkStartTime, 
                        p.WorkEndTime, 
                        p.PolicyType, 
                        p.CriticalHours, 
                        p.Action)
                );

            await foreach (var policy in listedPolicies)
            {
                yield return policy;
            }
        }

        public async Task<ApiResponse<GetPolicyResponseDto>> GetPolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            var policyDto = await _unitOfWork
                .Repository
                .GetSingleAsync<Policy, GetPolicyResponseDto>(
                    true,
                    p => p.Id == id,
                    p => new GetPolicyResponseDto(
                        p.Id, 
                        p.PolicyName,
                        p.WorkStartTime, 
                        p.WorkEndTime, 
                        p.PolicyType,
                        p.CriticalHours,
                        p.Action),
                    cancellationToken
                );

            if (policyDto != null)
            {
                return ApiResponse<GetPolicyResponseDto>.ReturnSuccessResponse(policyDto);
            }

            return ApiResponse<GetPolicyResponseDto>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);
        }

        public async Task<ApiResponse<object>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var policy = Policy.Create(addPolicyRequestDto);

            await _unitOfWork.Repository.AddAsync(policy, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.PolicyAddedSuccessfully);
        }

        public async Task<ApiResponse<object>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var targetPolicy = await _unitOfWork
                .Repository
                .GetSingleAsync<Policy>(
                    false,
                    x => x.Id == updatePolicyRequestDto.Id,
                    cancellationToken
                );

            if (targetPolicy == null)
                return ApiResponse<object>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);

            targetPolicy.Update(updatePolicyRequestDto);

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<object>.ReturnSuccessResponse(null, Messages.PolicyUpdatedSuccessfully);
            }

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.PolicyNotModified, HttpStatusCode.NotModified);
        }

        public async Task<ApiResponse<object>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default)
        {
            var targetPolicy = await _unitOfWork
                .Repository
                .GetSingleAsync<Policy>(
                    false,
                    x => x.Id == id,
                    cancellationToken
                );

            if (targetPolicy == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);
            }

            targetPolicy.SetIsDeletedToTrue();

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<object>.ReturnSuccessResponse(null, Messages.PolicyDeletedSuccessfully);
            }

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.PolicyNotModified, HttpStatusCode.NotModified);
        }
    }
}
