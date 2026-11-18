using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeDataImporterApi.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalRows = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ProcessedRows = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportRuns_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Office_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportRowResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ImportRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRowResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportRowResults_ImportRuns_ImportRunId",
                        column: x => x.ImportRunId,
                        principalTable: "ImportRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentOffice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentOffice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentOffice_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentOffice_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RoleLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportRowErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Field = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ErrorType = table.Column<string>(type: "text", nullable: false),
                    ImportRowResultId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRowErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportRowErrors_ImportRowResults_ImportRowResultId",
                        column: x => x.ImportRowResultId,
                        principalTable: "ImportRowResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSkill_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Organisations",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Shelby Limited Co.", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Dunder Mifflin Paper Co.", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0d92a7ff-df8c-49ad-9d4a-123ed27b6b1d"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Bookmaker", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("560bb0e8-1805-41b5-ab2f-3bb615b8c062"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Team Lead", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("813bf3a4-afa1-44a2-945c-8c7f441cd3a0"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Manager", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9a66ca52-56e5-4328-adca-1c29186c252d"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Associate", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1000001-0000-4000-8000-000000000001"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Leadership", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000002"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Public Speaking", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000003"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Sales", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000004"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "First Aid", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000005"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Customer Service", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000006"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Accounting", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000007"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Chili Cooking", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000008"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Inventory Management", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000009"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Forklift Operation", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000010"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Basketball", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000011"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Negotiation", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000012"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Excel", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000013"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Bookkeeping", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1000001-0000-4000-8000-000000000014"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Driving", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Code", "CreatedAt", "IsDeleted", "Name", "OrganisationId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("041d8943-30a3-4856-8638-3282e6d1c266"), "HR", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Human Resources", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("32a6098b-ca1b-485a-9414-2209209af7eb"), "SECU", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Security", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3f2a9c1e-5b7d-4e86-a1c4-9d8e7f6b5a01"), "ADM", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Administration", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("5d8b2e4f-9a1c-4c73-b0e6-3a4b5c6d7e04"), "QA", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Quality Assurance", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7c4d1b2a-8e3f-4a95-b6d0-1f2e3a4b5c02"), "SAL", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Sales", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("811a5ac8-d3bf-4a89-911f-e2af1c58e5be"), "HR", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Human Resources", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("94b3c5d7-1e2f-4a80-a6b9-0c1d2e3f4a06"), "WHS", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Warehouse", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1e5f3c7-2d94-4b68-8c1a-6e7f8091a203"), "ACC", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Accounting", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b462ba51-7b23-4ba9-a527-2aceff7a5b7e"), "OPS", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Operations", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c08ae784-b4ff-4cb3-ba68-339ee5d0fbed"), "FIN", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Finance", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d0256601-9d22-4972-928d-c1826747b481"), "LOG", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Logistics", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e2f6a8b0-4c7d-4d19-9e35-7b8c9d0e1f05"), "CS", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Customer Service", new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e98228a5-2023-42c4-b24e-e2265f1277e7"), "SEC", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, "Secretarial", new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Office",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "IsDeleted", "OrganisationId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0c31f42f-0f8b-4664-a50e-69fcb0bd53cc"), "Nashua", "USA", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("36c4af63-224c-4f5f-9b0c-cebb668fb02c"), "Scranton", "USA", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4b0e864b-3db3-420c-96a3-4b90a2a213db"), "London", "England", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("4b5cb182-6235-4a07-a1dd-dc978d1c3adf"), "Birmingham", "England", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("77337cd5-ac62-45f3-b476-b8777f8738f4"), "Manchester", "England", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f9b1b50c-a051-45ee-a0c7-a9362e26d4ea"), "Utica", "USA", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f9e516df-59d7-42bc-a7ed-76e87ec1b0ac"), "Stamford", "USA", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), false, new Guid("49b43e69-ed71-4ac2-80cf-f73acec0e11f"), new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOffice_DepartmentId",
                table: "DepartmentOffice",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOffice_OfficeId",
                table: "DepartmentOffice",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganisationId",
                table: "Departments",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OfficeId",
                table: "Employees",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganisationId_EmployeeNumber",
                table: "Employees",
                columns: new[] { "OrganisationId", "EmployeeNumber" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkill_EmployeeId",
                table: "EmployeeSkill",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkill_SkillId",
                table: "EmployeeSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportRowErrors_ImportRowResultId",
                table: "ImportRowErrors",
                column: "ImportRowResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportRowResults_ImportRunId",
                table: "ImportRowResults",
                column: "ImportRunId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportRuns_OrganisationId",
                table: "ImportRuns",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Office_OrganisationId",
                table: "Office",
                column: "OrganisationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentOffice");

            migrationBuilder.DropTable(
                name: "EmployeeSkill");

            migrationBuilder.DropTable(
                name: "ImportRowErrors");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "ImportRowResults");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ImportRuns");

            migrationBuilder.DropTable(
                name: "Organisations");
        }
    }
}
