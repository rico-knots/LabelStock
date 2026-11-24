using LabelStock.Core.Abstractions;
using LabelStock.Core.Models;

namespace LabelStock.Desktop.Demo;

/*
public sealed class DemoLabelService : ILabelService
{
    private readonly DemoStore _store;

    public DemoLabelService(DemoStore store) => _store = store;

    public Task<IReadOnlyList<LabelSummary>> SearchAsync(string query)
    {
        // An exact barcode match wins and returns only that label.
        var exact = _store.Labels.FirstOrDefault(l => l.Barcode == query);
        if (exact is not null)
        {
            return Task.FromResult<IReadOnlyList<LabelSummary>>(new[] { exact });
        }

        return Task.FromResult<IReadOnlyList<LabelSummary>>(Matching(query));
    }

    public Task<IReadOnlyList<LabelSummary>> GetRecentAsync(int count) =>
        Task.FromResult<IReadOnlyList<LabelSummary>>(_store.Labels.Take(count).ToList());

    public Task<IReadOnlyList<LabelSummary>> GetAllAsync(string? filter) =>
        Task.FromResult<IReadOnlyList<LabelSummary>>(
            string.IsNullOrWhiteSpace(filter) ? _store.Labels.ToList() : Matching(filter));

    public Task<LabelSummary> CreateAsync(string barcode, string name, int quantity)
    {
        if (_store.Labels.Any(l => l.Barcode == barcode))
        {
            throw new InvalidOperationException("This barcode is already in the system.");
        }

        var label = new LabelSummary(barcode, name, quantity);
        _store.Labels.Add(label);
        return Task.FromResult(label);
    }

    public Task DeleteAsync(int labelId)
    {
        return Task.CompletedTask;
    }

    private List<LabelSummary> Matching(string text) =>
        _store.Labels
            .Where(l => l.Name.Contains(text, StringComparison.OrdinalIgnoreCase)
                        || l.Barcode.Contains(text, StringComparison.OrdinalIgnoreCase))
            .ToList();
}
*/
