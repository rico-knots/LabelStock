using Avalonia.Controls;
using Avalonia.Threading;

namespace LabelStock.Desktop.Views;

public partial class AdminLoginView : UserControl
{
    public AdminLoginView()
    {
        InitializeComponent();

        AttachedToVisualTree += (_, _) => Dispatcher.UIThread.Post(() => PasswordBox.Focus());
    }
}
