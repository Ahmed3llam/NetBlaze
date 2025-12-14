using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class EmployeeAttendance : BaseEntity<long>
    {
        // Properties

        public DateOnly Date { get; set; }

        public TimeSpan Time { get; set; }

        public long UserId { get; set; }


        // Navigational Properties

        public virtual User User { get; set; }

        public ICollection<AttendancePolicyAction> AttendancePolicyActions { get; set; } = [];
    }
}
