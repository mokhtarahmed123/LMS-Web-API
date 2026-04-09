using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTableToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokeOn",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshToken",
                newName: "refreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedOn",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokedOn",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "refreshToken",
                table: "RefreshToken",
                newName: "Token");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokeOn",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
