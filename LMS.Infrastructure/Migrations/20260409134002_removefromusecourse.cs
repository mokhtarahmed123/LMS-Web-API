using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removefromusecourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "LastAccessedAt",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "ProgressPercent",
                table: "UserCourses");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UserCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessedAt",
                table: "UserCourses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgressPercent",
                table: "UserCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);




        }
    }
}
