using Avalonia.Controls;
using Avalonia.Threading;

namespace LabelStock.Desktop.Views;

public partial class NewLabelView : UserControl
{
    public NewLabelView()
    {
        InitializeComponent();

        AttachedToVisualTree += (_, _) => Dispatcher.UIThread.Post(() =>
        {
            // A scanned barcode is already filled in, so go to the name. Otherwise wait for a scan.
            if (string.IsNullOrEmpty(BarcodeBox.Text))
            {
                BarcodeBox.Focus();
            }
            else
            {
                NameBox.Focus();
            }
        });
    }
}
