using System.Text.Json.Serialization;
using EmployeeDataImporterApi.Domain;
using EmployeeDataImporterApi.Middleware;
using EmployeeDataImporterApi.Repository;
using EmployeeDataImporterApi.Services.Implementation;
using EmployeeDataImporterApi.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<EmployeeDataImporterContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddScoped<IEmployeeFileReader, ExcelEmployeeFileReader>();
builder.Services.AddScoped<IEmployeeFileReader, CsvEmployeeFileReader>();
builder.Services.AddScoped<EmployeeFileReaderFactory>();
builder.Services.AddScoped<ImportService>();
builder.Services.AddScoped<EmployeeValidator>();
builder.Services.AddScoped<SkillValidator>();
builder.Services.AddScoped<EmployeeDataValidator>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<FileExtensionMiddleware>();
app.MapControllers();

app.Run();