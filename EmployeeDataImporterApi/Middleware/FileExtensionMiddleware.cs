using EmployeeDataImporterApi.Domain.CustomAttributes;
using EmployeeDataImporterApi.Domain.CustomExceptions;
using MagicBytesValidator.Services.Http;

namespace EmployeeDataImporterApi.Middleware;

public class FileExtensionMiddleware
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".xlsx", ".csv" };

    private readonly RequestDelegate _next;

    public FileExtensionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata.GetMetadata<FileImport>() == null)
        {
            await _next(context);
            return;
        }

        var form = await context.Request.ReadFormAsync();
        var files = form.Files;

        if (files.Count == 0)
        {
            await _next(context);
            return;
        }

        var extensions = files
            .Select(f => Path.GetExtension(f.FileName).ToLowerInvariant())
            .ToList();
        
        var unsupported = extensions.Where(e => !AllowedExtensions.Contains(e)).Distinct().ToList();
        if (unsupported.Count > 0)
        {
            await Reject(context,
                $"Unsupported file type: {string.Join(", ", unsupported)}. Only .xlsx and .csv are allowed.");
            return;
        }

        if (extensions.Distinct().Count() > 1)
        {
            await Reject(context,
                "All files must have the same type: either all .xlsx or all .csv.");
            return;
        }

        if (extensions.Contains(".xlsx"))
        {
            var formFileTypeProvider = new MagicBytesValidator.Services.Http.FormFileTypeProvider();
            foreach (var file in files)
            {
                try
                {
                    await ValidateExcelFileAsync(file, formFileTypeProvider);
                }
                catch (InvalidFileContentException e)
                {
                    await Reject(context, e.Message);
                    return;
                }
            }
        }
        else
        {
            foreach (var file in files)
            {
                if (!await LooksLikeCsv(file))
                {
                    await Reject(context, $"File '{file.FileName}' is not a valid text/CSV file.");
                    return;
                }
            }
        }
        
        await _next(context);
    }

    private static async Task ValidateExcelFileAsync(IFormFile file, FormFileTypeProvider provider)
    {
        try
        {
            var fileType = await provider.FindValidatedTypeAsync(file,null, CancellationToken.None);
            if (fileType is null)
                throw new InvalidFileContentException($"File '{file.FileName}' is not a recognised Excel file.");
        }
        catch (MagicBytesValidator.Exceptions.Http.MimeTypeMismatchException ex)
        {
            throw new InvalidFileContentException(
                $"File '{file.FileName}' has an .xlsx extension, but its content is not an Excel workbook.", ex);
        }
    }
    
    private static async Task Reject(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
    
    private static async Task<bool> LooksLikeCsv(IFormFile file)
    {
        var buffer = new byte[8192];
        int read;

        await using (var stream = file.OpenReadStream())
            read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length));

        if (read == 0) return false;

        var start = read >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF ? 3 : 0;

        for (int i = start; i < read; i++)
        {
            var b = buffer[i];

            if (b == 0 || (b < 0x20 && b != (byte)'\t' && b != (byte)'\n' && b != (byte)'\r'))
                return false;
        }

        return true;
    }
}