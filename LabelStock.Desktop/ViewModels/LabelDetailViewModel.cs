using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Core.Models;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Screen 2: one label, with big buttons to change how many are in stock.</summary>
public partial class LabelDetailViewModel : ViewModelBase
{
    private readonly IStockService _stock;
    private readonly Navigator _navigator;
    private int _labelId;

    public LabelDetailViewModel(IStockService stock, Navigator navigator)
    {
        _stock = stock;
        _navigator = navigator;
    }

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private string _barcode = "";

    /// <summary>The number that is stored right now.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChange))]
    [NotifyPropertyChangedFor(nameof(ChangeText))]
    [NotifyPropertyChangedFor(nameof(SavedText))]
    private int _savedQuantity;

    /// <summary>The number on screen, which may not be saved yet.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChange))]
    [NotifyPropertyChangedFor(nameof(ChangeText))]
    private int _quantity;

    /// <summary>The same number as text, for the box the user can type in.</summary>
    [ObservableProperty]
    private string _quantityText = "0";

    [ObservableProperty]
    private bool _justSaved;

    [ObservableProperty]
    private string _error = "";
    public bool HasChange => Quantity != SavedQuantity;

    public string ChangeText
    {
        get
        {
            var difference = Quantity - SavedQuantity;
            var sign = difference > 0 ? "+" : "";
            return $"It was {SavedQuantity}, now {Quantity} ({sign}{difference}). Press Save to keep this.";
        }
    }

    public string SavedText => $"Saved. {Name}: {SavedQuantity} in stock.";

    /// <summary>Called by whoever opens this screen.</summary>
    public void Start(LabelSummary label)
    {
        _labelId = label.Id;
        Name = label.Name;
        Barcode = label.Barcode;
        SavedQuantity = label.Quantity;
        Quantity = label.Quantity;
        JustSaved = false;
    }

    partial void OnQuantityChanged(int value)
    {
        JustSaved = false;
        Error = "";

        var text = value.ToString();
        if (QuantityText != text)
        {
            QuantityText = text;
        }
    }

    partial void OnQuantityTextChanged(string value)
    {
        // Typed numbers are accepted when they are whole numbers of 0 or more.
        if (int.TryParse(value, out var number) && number >= 0)
        {
            Quantity = number;
        }
    }

    [RelayCommand]
    private void Decrease()
    {
        if (Quantity > 0)
        {
            Quantity--;
        }
    }

    [RelayCommand]
    private void Increase() => Quantity++;

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            await _stock.SetQuantityAsync(_labelId, Quantity);
            SavedQuantity = Quantity;
            JustSaved = true;
        }
        catch (Exception)
        {
            Error = "Could not save. Please try again.";
        }
    }

    [RelayCommand]
    private void Back() => _navigator.GoTo<ScanViewModel>();
}
