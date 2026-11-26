
namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeFileReaderFactory
{
    private readonly IEnumerable<FileReaderBase> _fileReaders;

    public EmployeeFileReaderFactory(IEnumerable<FileReaderBase> fileReaders)
    {
        _fileReaders = fileReaders;
    }

    public FileReaderBase GetFileReader(string extension)
    {
        var reader = _fileReaders.FirstOrDefault(r => r.CanReadFile(extension));
        if (reader == null)
            throw new NotSupportedException(
                $"Files with extension '{extension}' are not supported. Use .xlsx or .csv.");        
        return reader;
    }
}