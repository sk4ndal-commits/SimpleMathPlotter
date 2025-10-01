using Avalonia.Controls;

namespace SimpleMathPlotter.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUFG
        this.AttachDevTools();
#endif
    }
}