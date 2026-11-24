namespace LabelStock.Core.Abstractions;

public interface IAuthService
{
    Task<bool> VerifyPasswordAsync(string password);
    Task<bool> PasswordExistsAsync();
    Task SetPasswordAsync(string password);
}
