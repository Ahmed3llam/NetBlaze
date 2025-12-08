using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.General;
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

        public async Task<ApiResponse<PaginatedList<GetListedPolicyResponseDto>>> GetListedPolices(PaginateRequestDto paginateRequestDto)
        {
            var listedPolicies = _unitOfWork
                .Repository
                .GetQueryable<Policy>()
                .Select(p => new GetListedPolicyResponseDto(
                        p.Id,
                        p.PolicyName,
                        p.WorkStartTime,
                        p.WorkEndTime,
                        p.PolicyType,
                        p.CriticalHours,
                        p.Action,
                        p.IsActive));

            var result = await PaginatedList<GetListedPolicyResponseDto>.CreateAsync(listedPolicies, paginateRequestDto.PageNumber, paginateRequestDto.PageSize);

            return ApiResponse<PaginatedList<GetListedPolicyResponseDto>>.ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<GetPolicyResponseDto>> GetPolicyByIdAsync(long id, CancellationToken cancellationToken = default)
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

        public async Task<ApiResponse<long>> AddPolicyAsync(AddPolicyRequestDto addPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var policy = new Policy
            {
                PolicyName = addPolicyRequestDto.PolicyName,
                WorkStartTime = addPolicyRequestDto.WorkStartTime,
                WorkEndTime = addPolicyRequestDto.WorkEndTime,
                CriticalHours = addPolicyRequestDto.CriticalHours,
                PolicyType = addPolicyRequestDto.PolicyType,
                Action = addPolicyRequestDto.Action
            };

            await _unitOfWork.Repository.AddAsync(policy, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<long>.ReturnSuccessResponse(policy.Id, Messages.PolicyAddedSuccessfully);
        }

        public async Task<ApiResponse<long>> UpdatePolicyAsync(UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var targetPolicy = await _unitOfWork
                .Repository
                .GetSingleAsync<Policy>(
                    false,
                    x => x.Id == updatePolicyRequestDto.Id,
                    cancellationToken
                );

            if (targetPolicy == null)
                return ApiResponse<long>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);

            //ToDo: update only sent data
            targetPolicy.PolicyName = updatePolicyRequestDto.PolicyName;
            targetPolicy.WorkStartTime = updatePolicyRequestDto.WorkStartTime;
            targetPolicy.WorkEndTime = updatePolicyRequestDto.WorkEndTime;
            targetPolicy.CriticalHours = updatePolicyRequestDto.CriticalHours;
            targetPolicy.PolicyType = updatePolicyRequestDto.PolicyType;
            targetPolicy.Action = updatePolicyRequestDto.Action;

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<long>.ReturnSuccessResponse(targetPolicy.Id, Messages.PolicyUpdatedSuccessfully);
            }

            return ApiResponse<long>.ReturnSuccessResponse(targetPolicy.Id, Messages.PolicyNotModified, HttpStatusCode.NotModified);
        }

        public async Task<ApiResponse<long>> DeletePolicyAsync(long id, CancellationToken cancellationToken = default)
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
                return ApiResponse<long>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);
            }

            targetPolicy.ToggleIsActive();
            targetPolicy.SetIsDeletedToTrue();

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<long>.ReturnSuccessResponse(targetPolicy.Id, Messages.PolicyDeletedSuccessfully);
            }

            return ApiResponse<long>.ReturnSuccessResponse(targetPolicy.Id, Messages.PolicyNotModified, HttpStatusCode.NotModified);
        }
    }
}
