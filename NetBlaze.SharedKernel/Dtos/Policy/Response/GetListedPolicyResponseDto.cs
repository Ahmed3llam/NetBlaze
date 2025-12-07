using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Policy.Response
{
    public sealed record GetListedPolicyResponseDto(
        long Id,
        string Name,
        DateTime WorkStartTime,
        DateTime WorkEndTime,
        PolicyType PolicyType,
        int CriticalHours,
        double Action
    );
}