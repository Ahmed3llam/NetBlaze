using Microsoft.EntityFrameworkCore.Migrations;
using NetBlaze.Infrastructure.Views;

#nullable disable

namespace NetBlaze.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRandomCheckReportView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(VwRandomCheckReportMg.Up());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(VwRandomCheckReportMg.Down());
        }
    }
}
