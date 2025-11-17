using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KocurmeApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixExamIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "Contingents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contingents_ExamId",
                table: "Contingents",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contingents_Exams_ExamId",
                table: "Contingents",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contingents_Exams_ExamId",
                table: "Contingents");

            migrationBuilder.DropIndex(
                name: "IX_Contingents_ExamId",
                table: "Contingents");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Contingents");
        }
    }
}
