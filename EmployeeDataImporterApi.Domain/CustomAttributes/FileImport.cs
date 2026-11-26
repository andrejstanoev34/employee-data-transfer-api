namespace EmployeeDataImporterApi.Domain.CustomAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class FileImport : Attribute
{
    
}