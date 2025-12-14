using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addHoursValueToAttendancePolicyAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkStartTime",
                table: "Policies",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "WorkEndTime",
                table: "Policies",
                newName: "From");

            migrationBuilder.AddColumn<double>(
                name: "HoursValue",
                table: "AttendancePolicyActions",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursValue",
                table: "AttendancePolicyActions");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "Policies",
                newName: "WorkStartTime");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Policies",
                newName: "WorkEndTime");
        }
    }
}
