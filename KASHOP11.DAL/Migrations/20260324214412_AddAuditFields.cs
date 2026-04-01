using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP11.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createdById",
                table: "categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdOn",
                table: "categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "updatedById",
                table: "categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedOn",
                table: "categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_createdById",
                table: "categories",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_categories_updatedById",
                table: "categories",
                column: "updatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories",
                column: "createdById",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_updatedById",
                table: "categories",
                column: "updatedById",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_users_updatedById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_createdById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_updatedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "createdOn",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "updatedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "updatedOn",
                table: "categories");
        }
    }
}
