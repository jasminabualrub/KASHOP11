using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP11.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRestPasswordMigrationtouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeResetPassword",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetExpire",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeResetPassword",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PasswordResetExpire",
                table: "users");
        }
    }
}
