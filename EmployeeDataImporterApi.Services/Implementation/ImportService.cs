using EmployeeDataImporterApi.Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace EmployeeDataImporterApi.Services.Implementation;

public class ImportService
{
    private readonly EmployeeFileReaderFactory _fileReaderFactory;

    public ImportService(EmployeeFileReaderFactory fileReaderFactory)
    {
        _fileReaderFactory = fileReaderFactory;
    }

    public async Task<string> ImportData(List<IFormFile> files)
    {
        var extension = Path.GetExtension(files.First().FileName);
        var employeeFileReader = _fileReaderFactory.GetFileReader(extension);
        var importedData = new ImportedData();
        foreach (var formFile in files)
        {
            var fileName = formFile.FileName;
            employeeFileReader.ReadFile(formFile.OpenReadStream(), importedData, fileName);
        }
        
        
        
        return "";
    }
}