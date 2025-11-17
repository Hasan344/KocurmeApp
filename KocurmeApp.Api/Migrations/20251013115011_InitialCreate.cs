using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KocurmeApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheatingStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    IMT_GUN = table.Column<byte>(type: "tinyint", nullable: false),
                    V_BINA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IS_N1 = table.Column<int>(type: "int", nullable: false),
                    BINA = table.Column<short>(type: "smallint", nullable: false),
                    ZAL1 = table.Column<short>(type: "smallint", nullable: false),
                    FENN = table.Column<byte>(type: "tinyint", nullable: false),
                    FNADI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IS_N2 = table.Column<int>(type: "int", nullable: false),
                    ZAL2 = table.Column<short>(type: "smallint", nullable: false),
                    EYNI_D = table.Column<byte>(type: "tinyint", nullable: false),
                    EYNI_Y = table.Column<byte>(type: "tinyint", nullable: false),
                    EYNI_B = table.Column<byte>(type: "tinyint", nullable: false),
                    Y_OXSHAR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    T_OXSHAR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BAL1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BAL2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheatingStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheatingStudents_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    Z_KOD = table.Column<short>(type: "smallint", nullable: false),
                    XAR_DIL = table.Column<byte>(type: "tinyint", nullable: false),
                    NUMMETN = table.Column<byte>(type: "tinyint", nullable: true),
                    B_KOD = table.Column<short>(type: "smallint", nullable: false),
                    V_BINA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MERTEBE = table.Column<byte>(type: "tinyint", nullable: false),
                    KOL_SIRA = table.Column<byte>(type: "tinyint", nullable: false),
                    KOL_YER = table.Column<byte>(type: "tinyint", nullable: false),
                    KOL_SIRA0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOL_YER0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TUTUMU = table.Column<byte>(type: "tinyint", nullable: false),
                    TUTUMU0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GR_FL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KOL_ABT = table.Column<byte>(type: "tinyint", nullable: false),
                    KOL_NAZ = table.Column<byte>(type: "tinyint", nullable: false),
                    IMT_YERI = table.Column<byte>(type: "tinyint", nullable: false),
                    DIL = table.Column<byte>(type: "tinyint", nullable: false),
                    YASHKATEG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AADI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WAADI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODUL = table.Column<byte>(type: "tinyint", nullable: false),
                    OK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TEKTEK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MEKT_KOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INDMEKTEB = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheatingStudents_ExamId",
                table: "CheatingStudents",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ExamId",
                table: "Rooms",
                column: "ExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheatingStudents");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}
