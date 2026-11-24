using Avalonia.Controls;
using Avalonia.Threading;

namespace LabelStock.Desktop.Views;

public partial class ScanView : UserControl
{
    public ScanView()
    {
        InitializeComponent();

        // Keep the cursor in the search box so a barcode scan always lands there.
        AttachedToVisualTree += (_, _) => Dispatcher.UIThread.Post(() => QueryBox.Focus());
    }
}
