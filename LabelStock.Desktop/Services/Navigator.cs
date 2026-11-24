using CommunityToolkit.Mvvm.ComponentModel;
using LabelStock.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LabelStock.Desktop.Services;

/// <summary>
/// Switches the screen shown in the main window. A ViewModel asks for the next screen with
/// <c>GoTo&lt;SomeViewModel&gt;()</c>; the container builds it, and the main window shows its View.
/// </summary>
public partial class Navigator : ObservableObject
{
    private readonly IServiceProvider _services;

    public Navigator(IServiceProvider services) => _services = services;

    /// <summary>The screen currently shown.</summary>
    [ObservableProperty]
    private ViewModelBase? _current;

    /// <summary>Creates a fresh screen of type <typeparamref name="T"/>, shows it and returns it so the caller can pass it data.</summary>
    public T GoTo<T>() where T : ViewModelBase
    {
        var page = _services.GetRequiredService<T>();
        Current = page;
        return page;
    }
}
