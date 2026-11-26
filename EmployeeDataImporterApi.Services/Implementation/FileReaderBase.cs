using EmployeeDataImporterApi.Domain.Dto;

namespace EmployeeDataImporterApi.Services.Implementation;

public abstract class FileReaderBase
{

    public abstract void ReadFile(Stream stream, ImportedData importedData, string fileName);
    
    public abstract bool CanReadFile(string extension);
    
    protected bool HasHeaders(HashSet<string> presentHeaders, List<string> messages, string fileType, params string[] requiredHeaders )
    {
        var missingHeaders = requiredHeaders.Where(h => !presentHeaders.Contains(h)).ToList();
        foreach (var h in missingHeaders)
        {
            messages.Add($"The header {h} is missing from {fileType} file.");
        }

        return missingHeaders.Count == 0;
    }
}