using LabelStock.Core.Models;

namespace LabelStock.Core.Abstractions;

public interface IBackupService
{
    /// <summary>The outcome of the latest backup attempt.</summary>
    BackupStatus Status { get; }

    /// <summary>Raised whenever <see cref="Status"/> changes.</summary>
    event EventHandler? StatusChanged;

    Task BackupNowAsync(CancellationToken cancellationToken = default);
}
