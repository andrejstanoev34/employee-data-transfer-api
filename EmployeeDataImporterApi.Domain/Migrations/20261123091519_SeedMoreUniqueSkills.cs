using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeDataImporterApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreUniqueSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1000001-0000-4000-8000-000000000015"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Security", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000016"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Logistics Planning", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000017"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Whiskey Tasting", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000018"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Horse Racing", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("a1000001-0000-4000-8000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("a1000001-0000-4000-8000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("a1000001-0000-4000-8000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: new Guid("a1000001-0000-4000-8000-000000000018"));
        }
    }
}
