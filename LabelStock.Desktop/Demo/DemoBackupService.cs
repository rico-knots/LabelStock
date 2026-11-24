using LabelStock.Core.Abstractions;
using LabelStock.Core.Models;

namespace LabelStock.Desktop.Demo;

/// <summary>Demo only: pretends to back up. Nothing is written anywhere.</summary>
public sealed class DemoBackupService : IBackupService
{
    public BackupStatus Status { get; private set; } =
        new(IsOk: true, LastSuccess: DateTimeOffset.Now.AddHours(-6), Problem: null);

    public event EventHandler? StatusChanged;

    public async Task BackupNowAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(400, cancellationToken);
        Status = new BackupStatus(IsOk: true, LastSuccess: DateTimeOffset.Now, Problem: null);
        StatusChanged?.Invoke(this, EventArgs.Empty);
    }
}
