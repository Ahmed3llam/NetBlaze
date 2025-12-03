using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Entities
{
    public class Vacation : BaseEntity<long>
    {
        // Properties

        public DayOfWeek Day { get; set; }

        public VacationType VacationType { get; set; }

        public DateTimeOffset VacationDate { get; set; }

        public DateTimeOffset AlternativeDate { get; set; }

        public string Description { get; set; }
    }
}
