using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Screen 4: the admin password.</summary>
public partial class AdminLoginViewModel : ViewModelBase
{
    private readonly IAuthService _auth;
    private readonly Navigator _navigator;

    public AdminLoginViewModel(IAuthService auth, Navigator navigator)
    {
        _auth = auth;
        _navigator = navigator;
    }

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _error = "";

    [RelayCommand]
    private async Task UnlockAsync()
    {
        if (await _auth.VerifyPasswordAsync(Password))
        {
            _navigator.GoTo<AdminViewModel>();
            return;
        }

        Error = "That password is not right. Please try again.";
        Password = "";
    }

    [RelayCommand]
    private void Cancel() => _navigator.GoTo<ScanViewModel>();
}
