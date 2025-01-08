using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tedx.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPresentedBefore",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdeaCategory",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdeaDescription",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhyIdea",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPresentedBefore",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdeaCategory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdeaDescription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WhyIdea",
                table: "Users");
        }
    }
}
