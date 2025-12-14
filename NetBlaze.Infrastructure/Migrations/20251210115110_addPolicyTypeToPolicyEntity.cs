using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPolicyTypeToPolicyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "Policies",
                newName: "ActionValue");

            migrationBuilder.AddColumn<int>(
                name: "PolicyAction",
                table: "Policies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyAction",
                table: "Policies");

            migrationBuilder.RenameColumn(
                name: "ActionValue",
                table: "Policies",
                newName: "Action");
        }
    }
}
