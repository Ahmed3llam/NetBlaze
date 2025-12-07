using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

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


        // Domain Methods

        public static Policy Create<TDto>(TDto policyEntityDto) where TDto : class
        {
            return ReflectionMapper.MapToNew<TDto, Policy>(policyEntityDto);
        }

        public void Update<TDto>(TDto policyEntityDto) where TDto : class
        {
            this.MapToExisting(policyEntityDto);
        }
    }
}
