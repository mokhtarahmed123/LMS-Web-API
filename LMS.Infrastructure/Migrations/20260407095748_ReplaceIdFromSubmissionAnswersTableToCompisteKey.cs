using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIdFromSubmissionAnswersTableToCompisteKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionAnswers_SubmissionId",
                table: "SubmissionAnswers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SubmissionAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers",
                columns: new[] { "SubmissionId", "QuestionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SubmissionAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswers_SubmissionId",
                table: "SubmissionAnswers",
                column: "SubmissionId");
        }
    }
}
