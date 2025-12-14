using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Policy.Response
{
    public sealed record GetListedPolicyResponseDto(
        long Id,
        string Name,
        TimeSpan From,
        TimeSpan To,
        PolicyType PolicyType,
        PolicyAction PolicyAction,
        int CriticalHours,
        double ActionValue,
        bool IsActive
    );
}