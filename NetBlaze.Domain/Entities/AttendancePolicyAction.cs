using NetBlaze.Domain.Common;

namespace NetBlaze.Domain.Entities
{
    public class AttendancePolicyAction : BaseEntity<long>
    {
        // Properties

        public bool IsApplied { get; set; }

        public string? Clarification { get; set; }

        public long AttendanceId { get; set; }

        public long PolicyId { get; set; }


        // Navigational Properties

        public EmployeeAttendance Attendance { get; set; }

        public Policy Policy { get; set; }
    }
}
