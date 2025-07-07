using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace college_project.Migrations
{
    /// <inheritdoc />
    public partial class edit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificateImageBase64",
                table: "Certificates",
                newName: "CertificateImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificateImagePath",
                table: "Certificates",
                newName: "CertificateImageBase64");
        }
    }
}
