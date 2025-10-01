using Avalonia;
using Avalonia.Collections;
using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

public class GraphViewModel : ViewModelBase
{
    public AvaloniaList<Point> GraphPoints { get; } = [];
    
    public void SetGraphPoints(IEnumerable<Point> points)
    {
        GraphPoints.Clear();
        foreach (var point in points) GraphPoints.Add(point);
    }
}