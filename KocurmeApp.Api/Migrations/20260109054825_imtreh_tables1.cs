using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KocurmeApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class imtreh_tables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "imtreh",
                columns: table => new
                {
                    exam_id = table.Column<short>(type: "smallint", nullable: true),
                    V_NUM = table.Column<int>(type: "int", nullable: true),
                    QIYMET = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SOY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ADI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BABA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AGE = table.Column<short>(type: "smallint", nullable: true),
                    SERIYA_P = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NUM_PASP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BITIR_UN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IXTISASI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NAMIZ_M = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOKTOR_M = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ELMI_DER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IS_YERI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UNVAN = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BOLME = table.Column<short>(type: "smallint", nullable: true),
                    TEL_EV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TEL_IS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TEL_EHT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TARIX = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VEZIFESI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SECHILDI = table.Column<bool>(type: "bit", nullable: true),
                    num_exam = table.Column<byte>(type: "tinyint", nullable: true),
                    cinsi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reg_id = table.Column<byte>(type: "tinyint", nullable: true),
                    Rayon_id = table.Column<byte>(type: "tinyint", nullable: true),
                    SelectedDistrictForExam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    contract_numb = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    contract_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "imtrehbina",
                columns: table => new
                {
                    exam_id = table.Column<short>(type: "smallint", nullable: true),
                    i_r = table.Column<int>(type: "int", nullable: true),
                    VN = table.Column<byte>(type: "tinyint", nullable: true),
                    v_bina = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B_KOD = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "imtreh");

            migrationBuilder.DropTable(
                name: "imtrehbina");
        }
    }
}
