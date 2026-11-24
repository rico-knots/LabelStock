using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelStock.Core.Abstractions;
using LabelStock.Core.Models;
using LabelStock.Desktop.Services;

namespace LabelStock.Desktop.ViewModels;

/// <summary>Screen 1: scan or type a barcode / name, and see the recently scanned labels.</summary>
public partial class ScanViewModel : ViewModelBase
{
    // A scanned code of at least this many digits that matches nothing is offered as a new label.
    private const int MinBarcodeLength = 6;
    private const string RecentHeader = "Recently scanned";

    private readonly ILabelService _labels;
    private readonly Navigator _navigator;

    public ScanViewModel(ILabelService labels, Navigator navigator)
    {
        _labels = labels;
        _navigator = navigator;
        _ = LoadRecentAsync();
    }

    /// <summary>What is in the search box. A barcode scanner types here and presses Enter.</summary>
    [ObservableProperty]
    private string _query = "";

    [ObservableProperty]
    private string _listHeader = RecentHeader;

    /// <summary>A short problem message, empty when there is nothing to say.</summary>
    [ObservableProperty]
    private string _message = "";

    public ObservableCollection<LabelRowViewModel> Items { get; } = new();

    [RelayCommand]
    private async Task SearchAsync()
    {
        var text = Query.Trim();
        Message = "";
        if (text.Length == 0)
        {
            return;
        }

        try
        {
            var found = await _labels.SearchAsync(text);

            if (found.Count == 1)
            {
                Open(found[0]);
            }
            else if (found.Count == 0)
            {
                if (LooksLikeBarcode(text))
                {
                    // Unknown barcode: go straight to the "new label" form with it filled in.
                    _navigator.GoTo<NewLabelViewModel>().Start(text);
                }
                else
                {
                    Message = $"Nothing found for “{text}”.";
                }
            }
            else
            {
                ListHeader = $"Results for “{text}”";
                ShowRows(found);
            }
        }
        catch (Exception)
        {
            Message = "Something went wrong while searching. Please try again.";
        }
    }

    [RelayCommand]
    private void NewLabel() => _navigator.GoTo<NewLabelViewModel>().Start(null);

    private async Task LoadRecentAsync()
    {
        try
        {
            ShowRows(await _labels.GetRecentAsync(4));
        }
        catch (Exception)
        {
            Message = "Could not load the recent labels.";
        }
    }

    private void ShowRows(IEnumerable<LabelSummary> labels)
    {
        Items.Clear();
        foreach (var label in labels)
        {
            Items.Add(new LabelRowViewModel(label, open: row => Open(row.Label)));
        }
    }

    private void Open(LabelSummary label) => _navigator.GoTo<LabelDetailViewModel>().Start(label);

    private static bool LooksLikeBarcode(string text) =>
        text.Length >= MinBarcodeLength && text.All(char.IsDigit);
}
