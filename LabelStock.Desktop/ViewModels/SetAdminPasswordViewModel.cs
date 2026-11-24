using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Shown the first time someone opens Admin, when no admin password exists yet.</summary>
public partial class SetAdminPasswordViewModel : ViewModelBase
{
    private const int MinimumLength = 6;

    private readonly IAuthService _auth;
    private readonly Navigator _navigator;

    public SetAdminPasswordViewModel(IAuthService auth, Navigator navigator)
    {
        _auth = auth;
        _navigator = navigator;
    }

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _confirmPassword = "";

    /// <summary>Lets the user see what they type, which helps when typing a new password twice.</summary>
    [ObservableProperty]
    private bool _showPassword;

    [ObservableProperty]
    private string _error = "";

    public string RequirementText => $"Use at least {MinimumLength} characters.";

    [RelayCommand]
    private async Task SaveAsync()
    {
        Error = "";

        if (Password.Length < MinimumLength)
        {
            Error = $"The password needs at least {MinimumLength} characters.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            Error = "The two passwords are not the same. Please type the second one again.";
            ConfirmPassword = "";
            return;
        }

        try
        {
            await _auth.SetPasswordAsync(Password);

            // The person who just chose the password is let straight into admin.
            _navigator.GoTo<AdminViewModel>();
        }
        catch (Exception)
        {
            Error = "Could not save the password. Please try again.";
        }
    }

    [RelayCommand]
    private void Cancel() => _navigator.GoTo<ScanViewModel>();
}
