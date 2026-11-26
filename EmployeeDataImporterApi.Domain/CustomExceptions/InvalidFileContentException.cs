namespace EmployeeDataImporterApi.Domain.CustomExceptions;

public class InvalidFileContentException : Exception
{
    
    public InvalidFileContentException(){}
    
    public InvalidFileContentException(string message) :  base(message)
    {}
    public InvalidFileContentException(string message, Exception? inner = null)
        : base(message, inner) { }
}