using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Vacation.Requests
{
    public sealed record AddVacationRequestDto : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public VacationType VacationType { get; set; }

        public DayOfWeek? Day { get; set; }

        public DateTimeOffset? VacationDate { get; set; }

        public DateTimeOffset? AlternativeDate { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VacationType == VacationType.Weekly && !Day.HasValue)
            {
                yield return new ValidationResult(
                    Messages.FieldRequired,
                    new[] { nameof(Day) }
                );
            }

            if ((VacationType == VacationType.Monthly || VacationType == VacationType.Yearly)
                && !VacationDate.HasValue)
            {
                yield return new ValidationResult(
                    Messages.FieldRequired,
                    new[] { nameof(VacationDate) }
                );
            }
        }
    }
}
