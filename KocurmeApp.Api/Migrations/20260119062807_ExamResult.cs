using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KocurmeApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExamResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheatingRoomStatsResults",
                columns: table => new
                {
                    Zal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kolon5 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheatingRoomStatsResults");
        }
    }
}
