using Microsoft.EntityFrameworkCore;

namespace NetBlaze.Domain.Views
{
    [Keyless]
    public class RandomCheckReport
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsReplied { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
