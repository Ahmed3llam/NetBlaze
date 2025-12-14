using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.Domain.Views
{
    public class EmployeeLatenessReport
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long AttendanceId { get; set; }
        public DateOnly Date {  get; set; }
        public TimeSpan CheckInTime { get; set; }
        public long PolicyId { get; set; }
        public string PolicyName { get; set; }
        public PolicyAction PolicyAction { get; set; }
        public double ActionValue { get; set; }
        public int LateMinutes { get; set; }
        public bool IsActionTaken { get; set; }
        public double? AppliedHours { get; set; }
        public bool? ActionAppliedStatus { get; set; }
        public string? ActionClarification { get; set; }
    }
}
