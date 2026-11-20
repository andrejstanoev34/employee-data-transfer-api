using EmployeeDataImporterApi.Services.Interface;

namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeFileReaderFactory
{
    private readonly IEnumerable<IEmployeeFileReader> _fileReaders;

    public EmployeeFileReaderFactory(IEnumerable<IEmployeeFileReader> fileReaders)
    {
        _fileReaders = fileReaders;
    }

    public IEmployeeFileReader GetFileReader(string extension)
    {
        var reader = _fileReaders.FirstOrDefault(r => r.CanReadFile(extension));
        if (reader == null)
            throw new NotSupportedException(
                $"Files with extension '{extension}' are not supported. Use .xlsx or .csv.");        
        return reader;
    }
}