namespace LabelStock.Core.Settings;

public sealed class AppSettings
{
	public static string AppDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LabelStock");

	public static string SettingsFilePath => Path.Combine(AppDataDirectory, "settings.json");

	public string DatabaseFile { get; } = Path.Combine(AppDataDirectory, "labelstock.db");
	public string PasswordFile { get; } = Path.Combine(AppDataDirectory, "admin.hash");

	public string? BackupDirectory { get; set; }
}