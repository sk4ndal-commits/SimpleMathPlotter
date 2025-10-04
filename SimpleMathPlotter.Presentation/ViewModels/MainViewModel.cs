using System.ComponentModel;
using System.Globalization;
using Avalonia;
using SimpleMathPlotter.Core.Interfaces;
using SimpleMathPlotter.Core.Models;
using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

/// <summary>
/// ViewModel for the main application logic, coordinating function selection,
/// parameter settings, range settings, and graph plotting.
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly IFunctionEngine _functionEngine;
    private readonly IPersistenceService _persistenceService;

    #region ViewModels

    public FunctionSelectorViewModel FunctionSelectorViewModel { get; } = new();

    public ParameterSettingsViewModel ParameterSettingsViewModel { get; } =
        new();

    public RangeSettingsViewModel RangeSettingsViewModel { get; } = new();
    public GraphViewModel GraphViewModel { get; } = new();

    #endregion

    public RelayCommand ExportCommand { get; }

    public MainViewModel(
        IFunctionEngine functionEngine,
        IPersistenceService persistenceService,
        IExportService exportService)
    {
        _functionEngine = functionEngine;
        _persistenceService = persistenceService;

        ExportCommand = new RelayCommand(
            pathObj =>
            {
                if (pathObj is string path && !string.IsNullOrWhiteSpace(path))
                    exportService.Export(_currentYValues, path, _ymin, _ymax);
            },
            _ => _currentYValues.Count != 0);
        
        RegisterEventHandlers();
        Load();
        UpdatePlot();
    }
    
    ~MainViewModel()
    {
        UnregisterEventHandlers();
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
        if (!TryGetRanges(out var xmin, out var xmax, out var ymin, out var ymax))
            return;

        _ymin = ymin;
        _ymax = ymax;

        UpdateGraphViewModelRanges(xmin, xmax, ymin, ymax);

        var functionParameters = CreateFunctionParameters();
        _currentYValues = EvaluateFunction(functionParameters, xmin, xmax);

        var width = GraphViewModel.CanvasWidth;
        var height = GraphViewModel.CanvasHeight;

        GraphViewModel.SetGraphPoints(MapGraphPoints(_currentYValues, ymin, ymax, width, height));

        SaveState(functionParameters, xmin, xmax, ymin, ymax);
    }

    private bool TryGetRanges(out double xmin, out double xmax, out double ymin, out double ymax)
    {
        xmin = xmax = ymin = ymax = 0;
        if (!RangeSettingsViewModel.TryGetX(out xmin, out xmax)) return false;
        if (!RangeSettingsViewModel.TryGetY(out ymin, out ymax)) return false;
        if (ParameterSettingsViewModel.HasErrors) return false;
        return true;
    }

    private void UpdateGraphViewModelRanges(double xmin, double xmax, double ymin, double ymax)
    {
        GraphViewModel.Xmin = xmin;
        GraphViewModel.Xmax = xmax;
        GraphViewModel.Ymin = ymin;
        GraphViewModel.Ymax = ymax;
    }

    private FunctionParameters CreateFunctionParameters()
    {
        return new FunctionParameters
        {
            Amplitude = ParameterSettingsViewModel.AmplitudeValue,
            Frequency = ParameterSettingsViewModel.FrequencyValue,
            Phase = ParameterSettingsViewModel.PhaseValue,
            Offset = ParameterSettingsViewModel.OffsetValue
        };
    }

    private List<(double x, double y)> EvaluateFunction(FunctionParameters parameters, double xmin, double xmax)
    {
        return _functionEngine.Evaluate(
            FunctionSelectorViewModel.SelectedFunctionType,
            parameters,
            xmin,
            xmax,
            1200).ToList();
    }

    private static IEnumerable<Point> MapGraphPoints(
        List<(double x, double y)> points,
        double ymin, double ymax,
        double w, double h)
    {
        var xMin = points.Min(p => p.x);
        var xMax = points.Max(p => p.x);
        var xSpan = xMax - xMin;
        var ySpan = ymax - ymin;

        foreach (var (x, y) in points)
        {
            var sx = (x - xMin) / xSpan * w;
            var sy = h - (y - ymin) / ySpan * h;
            yield return new Point(sx, sy);
        }
    }

    private void SaveState(FunctionParameters parameters, double xmin, double xmax, double ymin, double ymax)
    {
        _persistenceService.Save(
            FunctionSelectorViewModel.SelectedFunctionType,
            parameters,
            xmin,
            xmax,
            ymin,
            ymax);
    }

    private void Load()
    {
        var loaded = _persistenceService.Load();
        if (loaded is null) return;

        var (type, parameters, xmin, xmax, ymin, ymax) = loaded.Value;
        FunctionSelectorViewModel.SelectedFunctionType = type;

        ParameterSettingsViewModel.Amplitude = parameters.Amplitude.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Frequency = parameters.Frequency.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Phase = parameters.Phase.ToString(CultureInfo.InvariantCulture);
        ParameterSettingsViewModel.Offset = parameters.Offset.ToString(CultureInfo.InvariantCulture);

        RangeSettingsViewModel.Xmin = xmin.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Xmax = xmax.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Ymin = ymin.ToString(CultureInfo.InvariantCulture);
        RangeSettingsViewModel.Ymax = ymax.ToString(CultureInfo.InvariantCulture);
    }

    private void RegisterEventHandlers()
    {
        RangeSettingsViewModel.PropertyChanged += OnSubViewModelPropertyChanged;
        ParameterSettingsViewModel.PropertyChanged += OnSubViewModelPropertyChanged;
        FunctionSelectorViewModel.PropertyChanged += OnSubViewModelPropertyChanged;
    }

    private void UnregisterEventHandlers()
    {
        RangeSettingsViewModel.PropertyChanged -= OnSubViewModelPropertyChanged;
        ParameterSettingsViewModel.PropertyChanged -= OnSubViewModelPropertyChanged;
        FunctionSelectorViewModel.PropertyChanged -= OnSubViewModelPropertyChanged;
    }

    private void OnSubViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!HasRelevantPropertyChanged(e)) return;
        
        if (CanPlot()) UpdatePlot();
    }

    private static bool HasRelevantPropertyChanged(PropertyChangedEventArgs e)
    {
        return e.PropertyName is nameof(RangeSettingsViewModel.Xmin) or
            nameof(RangeSettingsViewModel.Xmax) or
            nameof(RangeSettingsViewModel.Ymin) or
            nameof(RangeSettingsViewModel.Ymax) or
            nameof(ParameterSettingsViewModel.Amplitude) or
            nameof(ParameterSettingsViewModel.Frequency) or
            nameof(ParameterSettingsViewModel.Phase) or
            nameof(ParameterSettingsViewModel.Offset) or
            nameof(ParameterSettingsViewModel.HasErrors) or
            nameof(FunctionSelectorViewModel.SelectedFunctionType);
    }
}