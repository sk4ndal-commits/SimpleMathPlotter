using System.Globalization;
using Avalonia;
using SimpleMathPlotter.Core.Interfaces;
using SimpleMathPlotter.Core.Models;
using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IFunctionEngine _functionEngine;
    private readonly IPersistenceService _persistenceService;

    public FunctionSelectorViewModel FunctionSelectorViewModel { get; } = new();

    public ParameterSettingsViewModel ParameterSettingsViewModel { get; } =
        new();

    public RangeSettingsViewModel RangeSettingsViewModel { get; } = new();
    public GraphViewModel GraphViewModel { get; } = new();

    public RelayCommand UpdatePlotCommand { get; }
    public RelayCommand ExportCommand { get; }

    public MainViewModel(
        IFunctionEngine functionEngine,
        IPersistenceService persistenceService,
        IExportService exportService)
    {
        _functionEngine = functionEngine;
        _persistenceService = persistenceService;

        Load();
        UpdatePlot();

        UpdatePlotCommand = new RelayCommand(
            _ => UpdatePlot(),
            _ => CanPlot());

        ExportCommand = new RelayCommand(
            pathObj =>
            {
                if (pathObj is string path && !string.IsNullOrWhiteSpace(path))
                    exportService.Export(_currentYValues, path, _ymin, _ymax);
            }, 
            _ => _currentYValues.Count != 0);
    }

    private List<(double x, double y)> _currentYValues = [];
    private double _ymin = -5;
    private double _ymax = 5;

    private bool CanPlot() =>
        !ParameterSettingsViewModel.HasErrors &&
        RangeSettingsViewModel.TryGetX(out _, out _) &&
        RangeSettingsViewModel.TryGetY(out _, out _);

    private void UpdatePlot()
    {
        if (!RangeSettingsViewModel.TryGetX(out var xmin, out var xmax))
            return;
        if (!RangeSettingsViewModel.TryGetY(out var ymin, out var ymax))
            return;
        if (ParameterSettingsViewModel.HasErrors)
            return;

        _ymin = ymin;
        _ymax = ymax;

        GraphViewModel.Xmin = xmin;
        GraphViewModel.Xmax = xmax;
        GraphViewModel.Ymin = ymin;
        GraphViewModel.Ymax = ymax;
        var w = GraphViewModel.CanvasWidth;
        var h = GraphViewModel.CanvasHeight;

        var functionParameters = new FunctionParameters
        {
            Amplitude = ParameterSettingsViewModel.AmplitudeValue,
            Frequency = ParameterSettingsViewModel.FrequencyValue,
            Phase = ParameterSettingsViewModel.PhaseValue,
            Offset = ParameterSettingsViewModel.OffsetValue
        };

        var yValues = _functionEngine.Evaluate(
            FunctionSelectorViewModel.SelectedFunctionType,
            functionParameters,
            xmin,
            xmax,
            1200).ToList();

        _currentYValues = yValues;

        var xs = _currentYValues.Select(p => p.x).ToList();
        var xMin = xs.Min();
        var xMax = xs.Max();
        var xSpan = xMax - xMin;
        var ySpan = ymax - ymin;

        GraphViewModel.SetGraphPoints(Map());

        _persistenceService.Save(
            FunctionSelectorViewModel.SelectedFunctionType,
            functionParameters,
            xmin,
            xmax,
            ymin,
            ymax);
        return;

        IEnumerable<Point> Map()
        {
            foreach (var (x, y) in _currentYValues)
            {
                var sx = (x - xMin) / xSpan * w;
                var sy = h - (y - ymin) / ySpan * h;
                yield return new Point(sx, sy);
            }
        }
    }

    private void Load()
    {
        var loaded = _persistenceService.Load();
        if (loaded is null) return;

        var (type, parameters, xmin, xmax, ymin, ymax) = loaded.Value;
        FunctionSelectorViewModel.SelectedFunctionType = type;

        ParameterSettingsViewModel.Amplitude =
            parameters.Amplitude.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Frequency =
            parameters.Frequency.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Phase =
            parameters.Phase.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Offset =
            parameters.Offset.ToString(CultureInfo.InvariantCulture);

        RangeSettingsViewModel.Xmin =
            xmin.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Xmax =
            xmax.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Ymin =
            ymin.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Ymax =
            ymax.ToString(CultureInfo.InvariantCulture);
    }
}