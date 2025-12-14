namespace NetBlaze.Infrastructure.Views
{
    public static class VwAttendanceReportMg
    {
        public static string Up()
        {
            return @"
                CREATE VIEW EmployeeAttendanceReport AS
                SELECT
                    ea.UserId as EmployeeId,
                    u.DisplayName as EmployeeName,
                    u.DepartmentId,
                    d.Name AS DepartmentName,
                    ea.Date,
                    Min(ea.Time) AS CheckIn,
                    Max(ea.Time) AS CheckOut,
                    TIME_TO_SEC(TIMEDIFF(MAX(ea.Time), MIN(ea.Time)))/3600 AS HoursWorked
                FROM employeeattendances ea
                INNER JOIN users u ON u.Id = ea.UserId
                INNER JOIN departments d ON d.Id = u.DepartmentId
                GROUP BY ea.UserId, u.DisplayName, u.DepartmentId, d.Name, ea.Date;
            ";
        }

        public static string Down()
        {
            return @"DROP VIEW IF EXISTS EmployeeAttendanceReport;";
        }
    }
}
