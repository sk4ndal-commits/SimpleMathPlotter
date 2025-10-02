using Avalonia;
using Avalonia.Collections;
using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

/// <summary>
/// ViewModel for managing graph data and axis rendering.
/// </summary>
public class GraphViewModel : ViewModelBase
{
    public AvaloniaList<Point> GraphPoints { get; } = [];

    private double _xmin;

    public double Xmin
    {
        get => _xmin;
        set
        {
            if (SetField(ref _xmin, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    private double _xmax;

    public double Xmax
    {
        get => _xmax;
        set
        {
            if (SetField(ref _xmax, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    private double _ymin;

    public double Ymin
    {
        get => _ymin;
        set
        {
            if (SetField(ref _ymin, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    private double _ymax;

    public double Ymax
    {
        get => _ymax;
        set
        {
            if (SetField(ref _ymax, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    private double _canvasWidth = 800;

    public double CanvasWidth
    {
        get => _canvasWidth;
        set
        {
            if (SetField(ref _canvasWidth, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    private double _canvasHeight = 640;

    public double CanvasHeight
    {
        get => _canvasHeight;
        set
        {
            if (SetField(ref _canvasHeight, value))
            {
                UpdateAxes();
                UpdateLabels();
            }
        }
    }

    // --- Axis coordinates ---
    private Point _xAxisStart, _xAxisEnd, _yAxisStart, _yAxisEnd;

    public Point XAxisStart
    {
        get => _xAxisStart;
        private set => SetField(ref _xAxisStart, value);
    }

    public Point XAxisEnd
    {
        get => _xAxisEnd;
        private set => SetField(ref _xAxisEnd, value);
    }

    public Point YAxisStart
    {
        get => _yAxisStart;
        private set => SetField(ref _yAxisStart, value);
    }

    public Point YAxisEnd
    {
        get => _yAxisEnd;
        private set => SetField(ref _yAxisEnd, value);
    }

    // --- Label positions ---
    public double XminLabelLeft => 8;
    public double XminLabelTop => (_ymin < 0 && _ymax > 0) ? XAxisStart.Y + 8 : CanvasHeight - 24;

    public double XmaxLabelLeft => CanvasWidth - 38;
    public double XmaxLabelTop => (_ymin < 0 && _ymax > 0) ? XAxisStart.Y + 8 : CanvasHeight - 24;

    public double YminLabelLeft => (_xmin < 0 && _xmax > 0) ? YAxisStart.X - 18 : 8;
    public double YminLabelTop => CanvasHeight - 24;

    public double YmaxLabelLeft => (_xmin < 0 && _xmax > 0) ? YAxisStart.X - 18 : 8;
    public double YmaxLabelTop => 8;

    public void SetGraphPoints(IEnumerable<Point> points)
    {
        GraphPoints.Clear();
        foreach (var point in points)
            GraphPoints.Add(point);
    }

    private void UpdateAxes()
    {
        if (Math.Abs(_xmax - _xmin) < 1e-8 || Math.Abs(_ymax - _ymin) < 1e-8)
            return;

        // X axis at y=0 (only if visible in range)
        if (_ymin < 0 && _ymax > 0)
        {
            var y = MapY(0);
            XAxisStart = new Point(0, y);
            XAxisEnd = new Point(CanvasWidth, y);
        }
        else
        {
            XAxisStart = XAxisEnd = new Point(-1, -1); // hide off-canvas
        }

        // Y axis at x=0 (only if visible in range)
        if (_xmin < 0 && _xmax > 0)
        {
            var x = MapX(0);
            YAxisStart = new Point(x, 0);
            YAxisEnd = new Point(x, CanvasHeight);
        }
        else
        {
            YAxisStart = YAxisEnd = new Point(-1, -1);
        }

        return;

        double MapY(double y) =>
            CanvasHeight - (y - _ymin) / (_ymax - _ymin) * CanvasHeight;

        double MapX(double x) => (x - _xmin) / (_xmax - _xmin) * CanvasWidth;
    }

    private void UpdateLabels()
    {
        OnPropertyChanged(nameof(XminLabelLeft));
        OnPropertyChanged(nameof(XminLabelTop));
        OnPropertyChanged(nameof(XmaxLabelLeft));
        OnPropertyChanged(nameof(XmaxLabelTop));
        OnPropertyChanged(nameof(YminLabelLeft));
        OnPropertyChanged(nameof(YminLabelTop));
        OnPropertyChanged(nameof(YmaxLabelLeft));
        OnPropertyChanged(nameof(YmaxLabelTop));
    }
}