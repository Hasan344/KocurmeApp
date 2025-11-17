using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KocurmeApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class Contingents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contingents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMT_GUN = table.Column<byte>(type: "tinyint", nullable: true),
                    IMT_YERI = table.Column<byte>(type: "tinyint", nullable: true),
                    NUM_K = table.Column<byte>(type: "tinyint", nullable: true),
                    YASH_KATEQ = table.Column<byte>(type: "tinyint", nullable: true),
                    IZAHI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SEC = table.Column<byte>(type: "tinyint", nullable: true),
                    TIP_OTUR = table.Column<byte>(type: "tinyint", nullable: true),
                    SAYI = table.Column<short>(type: "smallint", nullable: true),
                    SAYI0 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contingents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contingents");
        }
    }
}
