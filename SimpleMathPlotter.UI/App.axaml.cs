using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SimpleMathPlotter.Core.Services;
using SimpleMathPlotter.Infrastructure.Export;
using SimpleMathPlotter.Infrastructure.Persistence;
using SimpleMathPlotter.Presentation.ViewModels;
using SimpleMathPlotter.UI.Views;

namespace SimpleMathPlotter.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var functionEngine = new FunctionEngine();
            var persistenceService = new JsonPersistenceService();
            var exporterService = new SvgExporterService();

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(
                    functionEngine, persistenceService, exporterService),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators
                .OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}