using Microsoft.EntityFrameworkCore;

namespace NetBlaze.Domain.Views
{
    public class EmployeeAttendanceReport
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan CheckOut { get; set; }
        public decimal HoursWorked { get; set; }
    }
}
