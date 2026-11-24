using LabelStock.Core.Models;

namespace LabelStock.Core.Abstractions;

public interface ILabelService
{
    /// <summary>
    /// Looks up labels by what the user typed or scanned. An exact barcode match returns
    /// just that label; anything else returns labels whose name or barcode contains the text.
    /// </summary>
    Task<IReadOnlyList<LabelSummary>> SearchAsync(string query);

    /// <summary>The labels to show under "Recently scanned".</summary>
    Task<IReadOnlyList<LabelSummary>> GetRecentAsync(int count);

    /// <summary>Every label, optionally filtered by name or barcode (admin list).</summary>
    Task<IReadOnlyList<LabelSummary>> GetAllAsync(string? filter);

    /// <summary>Creates a label. Throws <see cref="InvalidOperationException"/> if the barcode already exists.</summary>
    Task<LabelSummary> CreateAsync(string barcode, string name, int quantity);

    Task DeleteAsync(int entryId);
}
