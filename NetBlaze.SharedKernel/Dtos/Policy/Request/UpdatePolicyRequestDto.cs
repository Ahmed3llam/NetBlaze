using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetBlaze.SharedKernel.Dtos.Policy.Request
{
    //ToDo: make updates applying for items that user need
    public sealed record UpdatePolicyRequestDto
    {
        [IgnoreReflectionMapping]
        public long Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public string PolicyName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateTime? WorkStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateTime? WorkEndTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public PolicyType PolicyType { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public int CriticalHours { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public double Action { get; set; }

        public UpdatePolicyRequestDto() { }

        [JsonConstructor]
        public UpdatePolicyRequestDto(
            long id,
            string policyName,
            DateTime workStartTime,
            DateTime workEndTime,
            PolicyType policyType,
            int criticalHours,
            double action
        )
        {
            Id = id;
            PolicyName = policyName;
            WorkStartTime = workStartTime;
            WorkEndTime = workEndTime;
            PolicyType = policyType;
            CriticalHours = criticalHours;
            Action = action;
        }
    }
}
