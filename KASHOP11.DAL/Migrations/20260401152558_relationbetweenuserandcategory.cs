using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP11.DAL.Migrations
{
    /// <inheritdoc />
    public partial class relationbetweenuserandcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories",
                column: "createdById",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_createdById",
                table: "categories",
                column: "createdById",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
