using LabelStock.Core.Models;

namespace LabelStock.Desktop.Demo;

/// <summary>
/// In-memory sample data, shared by the demo services. Everything is lost when the app closes.
/// Delete the whole Demo folder once the real database services exist.
/// </summary>
public sealed class DemoStore
{
    private int _nextId = 7;

    public List<LabelSummary> Labels { get; } = new()
    {
        new(1, "8710400123456", "Gala apples", 48),
        new(2, "8710400123463", "Bananas", 120),
        new(3, "8710400123470", "Carrots", 12),
        new(4, "8710400123487", "Red onions", 0),
        new(5, "8710400123494", "Strawberries", 8),
        new(6, "8710400123500", "Vine tomatoes", 30),
    };

    public int NextId() => _nextId++;
}
