using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleMathPlotter.Presentation.ViewModels;

namespace SimpleMathPlotter.UI.Views;

public partial class GraphView : UserControl
{
    public GraphView()
    {
        InitializeComponent();
        DataContext = new GraphViewModel();
    }
}