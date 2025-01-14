using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tedx.Migrations
{
    /// <inheritdoc />
    public partial class addTwoFieldsToListenersUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdeaDescription",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

         

            migrationBuilder.AddColumn<bool>(
                name: "HasChildIn",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListenAboutEvent",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasChildIn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ListenAboutEvent",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "IdeaDescription",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);


        }
    }
}
