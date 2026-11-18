using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeDataImporterApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageToImportRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ImportRuns",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "ImportRuns");
        }
    }
}
