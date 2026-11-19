using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Services.Interface;

namespace EmployeeDataImporterApi.Services.Implementation;

public class CsvEmployeeFileReader : IEmployeeFileReader
{
    public ImportRunDto ReadFile(Stream stream)
    {
        throw new NotImplementedException();
    }
}