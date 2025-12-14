using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace NetBlaze.SharedKernel.Dtos.Attendance.Request
{
    public sealed record GetGetEmployeeLatenessRequestDto : PaginateRequestDto, IValidatableObject
    {
        public DateOnly? From { get; set; }

        public DateOnly? To { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To.HasValue && From.HasValue && To < From)
            {
                yield return new ValidationResult(
                    Messages.ToShouldBeAfterDateOfFrom,
                    new[] { nameof(To) }
                );
            }
        }
    }
}
