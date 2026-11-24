using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Models;

namespace LabelStock.Desktop.ViewModels;

/// <summary>
/// One row in a list of labels (the scan screen and the admin table).
/// The row owns its own commands, so the XAML can simply bind to them.
/// </summary>
public sealed class LabelRowViewModel
{
    public LabelRowViewModel(
        LabelSummary label,
        Action<LabelRowViewModel>? open = null,
        Action<LabelRowViewModel>? delete = null)
    {
        Label = label;
        OpenCommand = new RelayCommand(() => open?.Invoke(this));
        DeleteCommand = new RelayCommand(() => delete?.Invoke(this));
    }

    public LabelSummary Label { get; }

    public int Id => Label.Id;
    public string Name => Label.Name;
    public string Barcode => Label.Barcode;
    public string QuantityText => Label.Quantity.ToString();

    public bool InStock => Label.Quantity > 0;
    public bool OutOfStock => !InStock;

    public IRelayCommand OpenCommand { get; }
    public IRelayCommand DeleteCommand { get; }
}
