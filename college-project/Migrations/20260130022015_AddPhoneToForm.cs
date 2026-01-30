using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace college_project.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneToForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StudentRegistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StudentRegistrations");
        }
    }
}
