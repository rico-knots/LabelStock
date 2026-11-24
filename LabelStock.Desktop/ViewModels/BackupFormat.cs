using LabelStock.Core.Models;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Turns backup information into the short sentences shown on screen.</summary>
public static class BackupFormat
{
    /// <summary>"today 03:00", "yesterday 03:00" or "12 Oct 03:00".</summary>
    public static string When(DateTimeOffset moment)
    {
        var local = moment.ToLocalTime();
        var days = (DateTimeOffset.Now.Date - local.Date).Days;

        return days switch
        {
            0 => $"today {local:HH:mm}",
            1 => $"yesterday {local:HH:mm}",
            _ => $"{local:d MMM} {local:HH:mm}"
        };
    }

    /// <summary>The text for the header badge.</summary>
    public static string Badge(BackupStatus status) =>
        status.IsOk && status.LastSuccess is { } time
            ? $"Backup OK · {When(time)}"
            : $"Backup problem · {status.Problem ?? "no recent backup"}";

    /// <summary>The first line in the admin backup box.</summary>
    public static string LastBackupLine(BackupStatus status) =>
        status.LastSuccess is { } time
            ? $"Last backup: {When(time)}"
            : "No backup has been made yet";
}
