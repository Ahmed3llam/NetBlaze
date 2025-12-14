using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Attendance.Request
{
    public sealed record GetAllChecksRequestDto : PaginateRequestDto, IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly From { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public DateOnly To { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To < From)
            {
                yield return new ValidationResult(
                    Messages.ToShouldBeAfterDateOfFrom,
                    new[] { nameof(To) }
                );
            }
        }
    }
}
