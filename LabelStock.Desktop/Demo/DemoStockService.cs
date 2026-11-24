using LabelStock.Core.Abstractions;

namespace LabelStock.Desktop.Demo;

public sealed class DemoStockService : IStockService
{
    private readonly DemoStore _store;

    public DemoStockService(DemoStore store) => _store = store;

    public Task SetQuantityAsync(int labelId, int quantity)
    {
        var index = _store.Labels.FindIndex(l => l.Id == labelId);
        if (index >= 0)
        {
            // LabelSummary is immutable, so replace it with a copy that has the new quantity.
            _store.Labels[index] = _store.Labels[index] with { Quantity = Math.Max(0, quantity) };
        }

        return Task.CompletedTask;
    }
}
