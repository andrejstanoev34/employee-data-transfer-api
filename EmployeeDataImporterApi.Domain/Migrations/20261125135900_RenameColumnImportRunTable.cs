using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeDataImporterApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnImportRunTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ImportRuns",
                newName: "RunStatus");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ImportRowResults",
                newName: "RowStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunStatus",
                table: "ImportRuns",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "RowStatus",
                table: "ImportRowResults",
                newName: "Status");
        }
    }
}
