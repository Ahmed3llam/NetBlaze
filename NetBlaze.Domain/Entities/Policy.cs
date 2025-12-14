using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities
{
    public class Policy : BaseEntity<long>
    {
        // Properties

        public string PolicyName { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public PolicyType PolicyType { get; set; }
        public PolicyAction PolicyAction { get; set; }
        public int CriticalHours { get; set; }
        public double ActionValue {  get; set; }

        
        // Navigational Properties

        public ICollection<AttendancePolicyAction> AttendancePolicyActions { get; set; } = [];
    }
}
