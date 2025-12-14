namespace NetBlaze.Infrastructure.Views
{
    public static class VwEmployeeLatenessReportMg
    {
        public static string Up()
        {
            return @"
                CREATE VIEW EmployeeLatenessReport AS
                SELECT 
                    ea.UserId AS EmployeeId,
                    u.DisplayName AS EmployeeName,
                    u.DepartmentId,
                    d.Name AS DepartmentName,
                    ea.Date,
                    MIN(ea.Id) AS AttendanceId,
                    MIN(ea.Time) AS CheckInTime,
                    p.Id AS PolicyId,
                    p.PolicyName,
                    p.PolicyAction,
                    p.ActionValue,
                    TIMESTAMPDIFF(
                        MINUTE,
                        p.To,
                        MIN(ea.Time)
                    ) AS LateMinutes,
                    CASE 
                        WHEN ap.Id IS NOT NULL THEN 1
                        ELSE 0
                    END AS IsActionTaken,
                    ap.HoursValue AS AppliedHours,
                    ap.IsApplied AS ActionAppliedStatus,
                    ap.Clarification AS ActionClarification
                FROM employeeattendances ea
                JOIN users u 
                    ON u.Id = ea.UserId
                JOIN departments d
                    ON d.Id = u.DepartmentId
                JOIN policies p
                    ON p.PolicyType = 1
                LEFT JOIN attendancepolicyactions ap
                    ON ap.AttendanceId = ea.Id
                    AND ap.PolicyId = p.Id
                WHERE TIME(ea.Time) > p.To
                GROUP BY 
                    ea.UserId,
                    u.DisplayName,
                    u.DepartmentId,
                    d.Name,
                    ea.Date,
                    p.Id,
                    p.PolicyName,
                    p.PolicyAction,
                    p.ActionValue,
                    p.To,
                    ap.Id,
                    ap.HoursValue,
                    ap.IsApplied,
                    ap.Clarification
            ";
        }

        public static string Down()
        {
            return @"DROP VIEW IF EXISTS EmployeeLatenessReport;";
        }
    }
}
