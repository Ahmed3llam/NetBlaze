using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Policy.Response
{
    public sealed record GetPolicyResponseDto(
        long Id,
        string Name,
        TimeSpan From,
        TimeSpan To,
        PolicyType PolicyType,
        PolicyAction PolicyAction,
        int CriticalHours,
        double ActionValue
    );
}
