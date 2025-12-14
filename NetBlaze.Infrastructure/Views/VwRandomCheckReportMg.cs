
namespace NetBlaze.Infrastructure.Views
{
    public static class VwRandomCheckReportMg
    {
        public static string Up()
        {
            return @"
                CREATE VIEW RandomCheckReport AS
                SELECT
                    rc.UserId as EmployeeId,
                    u.DisplayName as EmployeeName,
                    u.DepartmentId,
                    d.Name as DepartmentName,
                    rc.Date,
                    rc.Time,
                    rc.IsReplied,
                    rc.RepliedDate
                FROM randomchecks rc
                INNER JOIN users u ON u.Id = rc.UserId
                INNER JOIN departments d ON d.Id = u.DepartmentId;
            ";
        }

        public static string Down()
        {
            return @"DROP VIEW IF EXISTS RandomCheckReport;";
        }
    }
}
