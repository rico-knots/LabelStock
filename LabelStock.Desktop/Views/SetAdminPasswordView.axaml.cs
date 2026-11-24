using Avalonia.Controls;
using Avalonia.Threading;

namespace LabelStock.Desktop.Views;

public partial class SetAdminPasswordView : UserControl
{
    public SetAdminPasswordView()
    {
        InitializeComponent();

        AttachedToVisualTree += (_, _) => Dispatcher.UIThread.Post(() => PasswordBox.Focus());
    }
}
