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
        if (!context.Request.Path.StartsWithSegments("/api/import")
            || !HttpMethods.IsPost(context.Request.Method)
            || !context.Request.HasFormContentType)
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

        await _next(context);
    }

    private static async Task Reject(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
}