using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Screen 5: backups, export, settings and the full label list with delete.</summary>
public partial class AdminViewModel : ViewModelBase
{
    private readonly ILabelService _labels;
    private readonly IBackupService _backup;
    private readonly Navigator _navigator;

    public AdminViewModel(ILabelService labels, IBackupService backup, Navigator navigator)
    {
        _labels = labels;
        _backup = backup;
        _navigator = navigator;

        RefreshBackup();
        _ = LoadRowsAsync();
    }

    public ObservableCollection<LabelRowViewModel> Rows { get; } = new();

    [ObservableProperty]
    private string _filterText = "";

    // ---- Backup box ----

    [ObservableProperty]
    private bool _backupOk;

    [ObservableProperty]
    private string _lastBackupText = "";

    [ObservableProperty]
    private string _backupNote = "";

    // ---- Messages ----

    /// <summary>Feedback after an admin action, empty when there is nothing to say.</summary>
    [ObservableProperty]
    private string _statusMessage = "";

    // ---- Delete confirmation ----

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPendingDelete))]
    [NotifyPropertyChangedFor(nameof(DeleteQuestion))]
    private LabelRowViewModel? _pendingDelete;

    public bool HasPendingDelete => PendingDelete is not null;

    public string DeleteQuestion =>
        PendingDelete is null ? "" : $"Delete “{PendingDelete.Name}”? This cannot be undone.";

    partial void OnFilterTextChanged(string value) => _ = LoadRowsAsync();

    // ---- Commands ----

    [RelayCommand]
    private async Task BackupNowAsync()
    {
        StatusMessage = "";
        try
        {
            await _backup.BackupNowAsync();
            StatusMessage = "Backup finished.";
        }
        catch (Exception)
        {
            StatusMessage = "The backup did not work. Check that the USB stick is in the laptop.";
        }

        RefreshBackup();
    }

    [RelayCommand]
    private void NewLabel() => _navigator.GoTo<NewLabelViewModel>().Start(null);

    [RelayCommand]
    private async Task ConfirmDeleteAsync()
    {
        if (PendingDelete is not { } row)
        {
            return;
        }

        try
        {
            await _labels.DeleteAsync(row.Id);
            Rows.Remove(row);
            StatusMessage = $"Deleted “{row.Name}”.";
        }
        catch (Exception)
        {
            StatusMessage = "Could not delete the label. Please try again.";
        }

        PendingDelete = null;
    }

    [RelayCommand]
    private void CancelDelete() => PendingDelete = null;

    [RelayCommand]
    private void Lock() => _navigator.GoTo<ScanViewModel>();

    // These buttons are in the design, but the features behind them are not built yet.
    [RelayCommand]
    private void Restore() => StatusMessage = "Restoring from a backup is not built yet.";

    [RelayCommand]
    private void Export() => StatusMessage = "Exporting to a spreadsheet is not built yet.";

    [RelayCommand]
    private void ChangePassword() => StatusMessage = "Changing the password is not built yet.";

    [RelayCommand]
    private void ChooseBackupFolder() => StatusMessage = "Choosing the backup folder is not built yet.";

    // ---- Helpers ----

    private async Task LoadRowsAsync()
    {
        try
        {
            var all = await _labels.GetAllAsync(string.IsNullOrWhiteSpace(FilterText) ? null : FilterText.Trim());

            Rows.Clear();
            foreach (var label in all)
            {
                Rows.Add(new LabelRowViewModel(
                    label,
                    open: row => _navigator.GoTo<LabelDetailViewModel>().Start(row.Label),
                    delete: row => PendingDelete = row));
            }
        }
        catch (Exception)
        {
            StatusMessage = "Could not load the labels.";
        }
    }

    private void RefreshBackup()
    {
        var status = _backup.Status;
        BackupOk = status.IsOk;
        LastBackupText = BackupFormat.LastBackupLine(status);
        BackupNote = status.IsOk
            ? "Saved to the USB stick. The 7 newest backups are kept."
            : status.Problem ?? "The last backup did not work.";
    }
}
