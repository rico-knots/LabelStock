using CommunityToolkit.Mvvm.ComponentModel;

namespace LabelStock.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Greeting { get; set; } = "Grote penis!";
}