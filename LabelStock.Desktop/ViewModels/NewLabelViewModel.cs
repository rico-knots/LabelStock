using Avalonia.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Core.Models;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Screen 3: add a label the system doesn't know yet.</summary>
public partial class NewLabelViewModel : ViewModelBase
{
    private readonly ILabelService _labels;
    private readonly Navigator _navigator;

    public NewLabelViewModel(ILabelService labels, Navigator navigator)
    {
        _labels = labels;
        _navigator = navigator;
    }

    [ObservableProperty]
    private string _barcode = "";

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private string _startQuantityText = "";

    [ObservableProperty]
    private string _error = "";

    /// <summary>True when the barcode was scanned on the previous screen and was not found.</summary>
    [ObservableProperty]
    private bool _isFromScan;

    /// <summary>Called by whoever opens this screen. Pass the scanned barcode, or null to start empty.</summary>
    public void Start(string? scannedBarcode)
    {
        Barcode = scannedBarcode ?? "";
        IsFromScan = !string.IsNullOrEmpty(scannedBarcode);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var barcode = Barcode.Trim();
        var name = Name.Trim();

        if (barcode.Length == 0)
        {
            Error = "Scan or type the barcode first.";
            return;
        }

        if (name.Length == 0)
        {
            Error = "Give the label a name.";
            return;
        }

        var quantity = 0;
        if (!string.IsNullOrWhiteSpace(StartQuantityText)
            && (!int.TryParse(StartQuantityText, out quantity) || quantity < 0))
        {
            Error = "How many you have must be a whole number, 0 or more.";
            return;
        }

        try
        {
            var created = await _labels.CreateAsync(
                barcode, name, quantity);

            _navigator.GoTo<LabelDetailViewModel>().Start(created);
        }
        catch (InvalidOperationException ex)
        {
            // For example: this barcode already exists.
            Error = ex.Message;
        }
        catch (Exception e)
        {
            Error = "Could not save the label. Please try again.";
            System.Diagnostics.Debug.WriteLine(e);
        }
    }

    [RelayCommand]
    private void Cancel() => _navigator.GoTo<ScanViewModel>();
}
