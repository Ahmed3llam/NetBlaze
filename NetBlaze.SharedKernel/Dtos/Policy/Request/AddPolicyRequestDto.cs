using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;
namespace NetBlaze.SharedKernel.Dtos.Policy.Request
{
    public sealed record AddPolicyRequestDto
    {
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
    }
}
