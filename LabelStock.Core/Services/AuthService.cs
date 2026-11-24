using LabelStock.Core.Abstractions;
using LabelStock.Core.Settings;
using Microsoft.AspNetCore.Identity;

namespace LabelStock.Core.Services;

public sealed class AuthService(AppSettings settings) : IAuthService
{
	private readonly PasswordHasher<object> _hasher = new();

	public async Task<bool> PasswordExistsAsync()
	{
		var storedHash = await ReadHashAsync();
		return !string.IsNullOrWhiteSpace(storedHash);
	}

	public async Task<bool> VerifyPasswordAsync(string password)
	{
		if (string.IsNullOrWhiteSpace(password))
			return false;

		var storedHash = await ReadHashAsync();
		if (string.IsNullOrWhiteSpace(storedHash))
			return false;

		var result = _hasher.VerifyHashedPassword(null!, storedHash, password);

		// Optional: automatically upgrade the stored hash if .NET's default iteration count increases
		if (result == PasswordVerificationResult.SuccessRehashNeeded)
		{
			await SetPasswordAsync(password);
			return true;
		}

		return result == PasswordVerificationResult.Success;
	}

	public async Task SetPasswordAsync(string password)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(password);

		var directory = Path.GetDirectoryName(settings.PasswordFile);
		if (!string.IsNullOrEmpty(directory))
		{
			Directory.CreateDirectory(directory);
		}

		var hash = _hasher.HashPassword(null!, password);
		await File.WriteAllTextAsync(settings.PasswordFile, hash);
	}

	private async Task<string?> ReadHashAsync()
	{
		if (!File.Exists(settings.PasswordFile))
			return null;

		var content = await File.ReadAllTextAsync(settings.PasswordFile);
		return content.Trim();
	}
}