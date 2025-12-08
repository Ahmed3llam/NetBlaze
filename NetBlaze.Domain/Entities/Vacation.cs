using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Domain.Entities
{
    public class Vacation : BaseEntity<long>
    {
        // Properties

        public DayOfWeek? Day { get; set; }

        public VacationType VacationType { get; set; }

        public DateTime? VacationDate { get; set; }

        public DateTime? AlternativeDate { get; set; }

        public string? Description { get; set; }
    }
}
