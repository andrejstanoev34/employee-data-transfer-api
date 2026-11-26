using System.Globalization;
using System.Text.RegularExpressions;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Common;

public static class Util
{
    
    public static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    
    private static readonly string[] DateFormats =
    {
        "yyyy-M-d", "yyyy/M/d", "yyyy.M.d",
        "d-M-yyyy",  "d/M/yyyy",  "d.M.yyyy"
    };
    
    private static readonly Dictionary<string, RoleLevel> LevelPrefixes = new()
    {
        ["junior"] = RoleLevel.Junior,
        ["mid"]    = RoleLevel.Mid,
        ["senior"] = RoleLevel.Senior,
        ["lead"]   = RoleLevel.Lead
    };
    
    
    public static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : Regex.Replace(value.Trim(), @"\s+", " ");

    public static string NormalizeTitle(string title) => Regex.Replace(title.Trim(), @"\s+", " ").ToLowerInvariant();

    public static (string BaseTitle, RoleLevel Level) SplitRoleTitle(string title)
    {
        var normalized = NormalizeTitle(title);
        var parts = normalized.Split(' ', 2);
        if (parts.Length == 2 && LevelPrefixes.TryGetValue(parts[0], out var level))
            return (parts[1], level);
        return (normalized, RoleLevel.Mid);
    }

    public static DateOnly? ParseDate(string text) =>
        DateOnly.TryParseExact(text, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;

    public static bool IsValidPhone(string phone)
    {
        if (!Regex.IsMatch(phone, @"^\+?[\d\s().\-]+$")) return false;
        var digits = phone.Count(char.IsDigit);
        return digits is >= 7 and <= 15;
    }

    public static bool IsEmpty(params string?[] requiredHeaders) =>
        requiredHeaders.All(v => v == null);
}