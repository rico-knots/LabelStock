using LabelStock.Core.Abstractions;

namespace LabelStock.Desktop.Demo;

/// <summary>
/// Demo only: the password lives in memory and starts empty, so the first click on Admin
/// shows the "set a password" screen. The real service stores a salted hash.
/// </summary>
public sealed class DemoAuthService : IAuthService
{
    private string? _password;

    public Task<bool> PasswordExistsAsync() => Task.FromResult(_password is not null);


    public Task SetPasswordAsync(string newPassword)
    {
        _password = newPassword;
        return Task.CompletedTask;
    }

    public Task<bool> VerifyPasswordAsync(string password) =>
        Task.FromResult(_password is not null && password == _password);
}
