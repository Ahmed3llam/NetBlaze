using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetBlaze.SharedKernel.Dtos.Vacation.Requests
{
    public sealed record UpdateVacationRequestDto : IValidatableObject
    {
        [IgnoreReflectionMapping]
        public long Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.FieldRequired))]
        public VacationType VacationType { get; set; }

        public DayOfWeek? Day { get; set; }

        public DateTime? VacationDate { get; set; }

        public DateTime? AlternativeDate { get; set; }

        public string? Description { get; set; }

        // Constructor
        public UpdateVacationRequestDto() { }

        [JsonConstructor]
        public UpdateVacationRequestDto(
            long id,
            VacationType vacationType,
            DayOfWeek? day,
            DateTime? vacationDate,
            DateTime? alternativeDate,
            string? description
        )
        {
            Id = id;
            VacationType = vacationType;
            VacationDate = vacationDate;
            AlternativeDate = alternativeDate;
            Description = description;
        }

        // Validation
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
