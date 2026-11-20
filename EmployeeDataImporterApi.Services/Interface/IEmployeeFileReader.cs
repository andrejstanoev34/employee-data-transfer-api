using EmployeeDataImporterApi.Domain.Dto;

namespace EmployeeDataImporterApi.Services.Interface;

public interface IEmployeeFileReader
{ 
    void ReadFile(Stream stream, ImportedData importedData, string fileName);
    bool CanReadFile(string extension);
}