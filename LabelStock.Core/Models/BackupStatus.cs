namespace LabelStock.Core.Models;

/// <summary>The result of the most recent backup, shown in the header of every screen.</summary>
/// <param name="IsOk">False when the last attempt failed or no recent backup exists.</param>
/// <param name="LastSuccess">When the last good backup finished, if there ever was one.</param>
/// <param name="Problem">A short plain-language reason when <paramref name="IsOk"/> is false.</param>
public sealed record BackupStatus(bool IsOk, DateTimeOffset? LastSuccess, string? Problem);
