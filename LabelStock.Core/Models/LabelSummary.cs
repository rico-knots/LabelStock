namespace LabelStock.Core.Models;

/// <summary>
/// A label together with its current stock, as the screens show it.
/// This is a read model for the UI; it is separate from the database entities.
/// </summary>
public sealed record LabelSummary(int Id, string Barcode, string Name, int Quantity);
