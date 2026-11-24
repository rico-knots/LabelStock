# LabelStock UI

Avalonia views and ViewModels for the five screens in the design. Folder names match the solution,
so copy each folder over the one with the same name.

## 1. Packages (run in the solution folder)

```
dotnet add LabelStock.Desktop package CommunityToolkit.Mvvm
dotnet add LabelStock.Desktop package Microsoft.Extensions.DependencyInjection
```

`LabelStock.Desktop` must also reference `LabelStock.Core` (Add > Reference in Rider).

## 2. Copy the files

- `LabelStock.Core/`    -> Models/ and Abstractions/ (new files)
- `LabelStock.Desktop/` -> App.axaml, App.axaml.cs, Styles/, Views/, ViewModels/, Services/, Demo/

These REPLACE the template's own App.axaml, App.axaml.cs, MainWindow.axaml(.cs), MainWindowViewModel.cs
and ViewModelBase.cs. The template's ViewLocator.cs is no longer used (the window maps ViewModels to
Views itself) and can be deleted.

## 3. Run

Set LabelStock.Desktop as the startup project and press Run.
The admin password in the demo is `admin`. Data is in memory and resets on every start.

## Not built yet (buttons exist, they only show a message)

Restore from a backup, Export to spreadsheet, Change admin password, Choose backup folder.

## Replacing the demo data

The `Demo/` folder holds in-memory versions of the services. When the real ones exist in
LabelStock.Core, change the "Data and logic" block in App.axaml.cs (ConfigureServices) to
`services.AddCore()` and delete the `Demo/` folder.
