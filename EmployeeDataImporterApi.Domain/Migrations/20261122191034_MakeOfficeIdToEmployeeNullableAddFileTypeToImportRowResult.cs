using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeDataImporterApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MakeOfficeIdToEmployeeNullableAddFileTypeToImportRowResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "ImportRowResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "OfficeId",
                table: "Employees",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ImportRowResults");

            migrationBuilder.AlterColumn<Guid>(
                name: "OfficeId",
                table: "Employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
