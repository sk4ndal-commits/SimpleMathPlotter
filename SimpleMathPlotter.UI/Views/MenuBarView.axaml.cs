using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleMathPlotter.Presentation.ViewModels;

namespace SimpleMathPlotter.UI.Views;

public partial class MenuBarView : UserControl
{
    public MenuBarView()
    {
        InitializeComponent();
        
    }

    private async void OnExportClick(object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        var top = TopLevel.GetTopLevel(this);
        if (top is not Window window) return;

        var saveFileDialog = new SaveFileDialog
        {
            Title = "Export as SVG",
            Filters =
            {
                new FileDialogFilter { Name = "SVG", Extensions = { "svg" } }
            },
            InitialFileName = "plot.svg"
        };

        var path = await saveFileDialog.ShowAsync(window);
        if (!string.IsNullOrWhiteSpace(path) &&
            DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.ExportCommand.Execute(path);
        }
    }
}