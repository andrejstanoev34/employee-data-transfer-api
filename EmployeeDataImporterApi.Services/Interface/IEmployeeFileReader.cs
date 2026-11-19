using EmployeeDataImporterApi.Domain.Dto;

namespace EmployeeDataImporterApi.Services.Interface;

public interface IEmployeeFileReader
{
    public ImportRunDto ReadFile(Stream stream);
}