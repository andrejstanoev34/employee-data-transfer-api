using EmployeeDataImporterApi.Domain.Dto;

namespace EmployeeDataImporterApi.Services.Interface;

public interface IEmployeeFileReader
{
    public ImportedData ReadFile(Stream stream);
}