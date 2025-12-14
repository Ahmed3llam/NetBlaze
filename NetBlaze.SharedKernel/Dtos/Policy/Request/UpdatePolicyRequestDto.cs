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
        public TimeSpan From { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public TimeSpan To { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public PolicyType PolicyType { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public PolicyAction PolicyAction { get; set; }
        public int CriticalHours { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public double ActionValue { get; set; }

        public UpdatePolicyRequestDto() { }

        [JsonConstructor]
        public UpdatePolicyRequestDto(
            long id,
            string policyName,
            TimeSpan from,
            TimeSpan to,
            PolicyType policyType,
            PolicyAction policyAction,
            int criticalHours,
            double actionValue
        )
        {
            Id = id;
            PolicyName = policyName;
            From = from;
            To = to;
            PolicyType = policyType;
            PolicyAction = policyAction;
            CriticalHours = criticalHours;
            ActionValue = actionValue;
        }
    }
}
