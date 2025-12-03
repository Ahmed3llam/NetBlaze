using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities
{
    public class Policy : BaseEntity<long>
    {
        // Properties

        public string PolicyName { get; set; }
        public DateTime WorkStartTime { get; set; }
        public DateTime WorkEndTime { get; set; }
        public PolicyType PolicyType { get; set; }
        public int CriticalHours { get; set; }
        public double Action {  get; set; }

        
        // Navigational Properties

        public ICollection<AttendancePolicyAction> AttendancePolicies { get; set; } = [];
    }
}
