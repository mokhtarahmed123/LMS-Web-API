using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusOfInstructorProfileFromBoolToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "InstructorProfiles");

            migrationBuilder.AddColumn<int>(
                name: "StatusOfInstructorProfile",
                table: "InstructorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusOfInstructorProfile",
                table: "InstructorProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "InstructorProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
