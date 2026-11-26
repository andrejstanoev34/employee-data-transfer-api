using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Common;

public static class DtoConversion
{
    public static ImportRunDto ToImportRunDto(this ImportRun importRun)
    {
        return new ImportRunDto()
        {
            ImportId = importRun.Id,
            FileName = importRun.FileName,
            OrganisationName = importRun.Organisation.Name,
            Status = importRun.RunStatus.ToString(),
            StartTime = importRun.StartTime,
            EndTime = importRun.EndTime,
            Message = importRun.Message,
            TotalRows =  importRun.TotalRows,
            ProcessedRows = importRun.ProcessedRows,
        };
    }

    public static List<ImportRunDto> ToImportRunDtoList(this List<ImportRun> importRuns)
    {
        var lista = new List<ImportRunDto>();

        foreach (var i in importRuns)
        {
            lista.Add(i.ToImportRunDto());
        }
        
        return lista;
    }
}