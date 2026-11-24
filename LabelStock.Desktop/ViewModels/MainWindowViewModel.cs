using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>The window around every screen: the header with the backup badge and the Admin button.</summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IBackupService _backup;
    private readonly IAuthService _auth;

    public MainWindowViewModel(Navigator navigator, IBackupService backup, IAuthService auth)
    {
        Navigator = navigator;
        _backup = backup;
        _auth = auth;

        // The Admin button is hidden while the admin screens are open.
        navigator.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Navigator.Current))
            {
                OnPropertyChanged(nameof(ShowAdminButton));
            }
        };

        backup.StatusChanged += (_, _) => RefreshBackup();
        RefreshBackup();
    }

    public Navigator Navigator { get; }

    public bool ShowAdminButton => Navigator.Current is not (AdminViewModel or AdminLoginViewModel or SetAdminPasswordViewModel);

    [ObservableProperty]
    private bool _backupOk;

    [ObservableProperty]
    private string _backupText = "";

    /// <summary>Asks for the password, or for choosing one the very first time.</summary>
    [RelayCommand]
    private async Task OpenAdminAsync()
    {
        if (await _auth.PasswordExistsAsync())
        {
            Navigator.GoTo<AdminLoginViewModel>();
        }
        else
        {
            Navigator.GoTo<SetAdminPasswordViewModel>();
        }
    }

    private void RefreshBackup()
    {
        BackupOk = _backup.Status.IsOk;
        BackupText = BackupFormat.Badge(_backup.Status);
    }
}
