using Microsoft.EntityFrameworkCore.Migrations;
using NetBlaze.Infrastructure.Views;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addEmployeeAttendanceSummaryView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(VwAttendanceReportMg.Up());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(VwAttendanceReportMg.Down());
        }
    }
}
