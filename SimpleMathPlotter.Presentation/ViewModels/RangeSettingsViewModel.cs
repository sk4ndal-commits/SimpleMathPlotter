using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

public class RangeSettingsViewModel : ViewModelBase
{
    #region Ranges

    private string _xmin = "-10";
    private string _xmax = "10";
    private string _ymin = "-10";
    private string _ymax = "10";

    public string Xmin
    {
        get => _xmin;
        set
        {
            if (SetField(ref _xmin, value)) ValidateX();
        }
    }

    public string Xmax
    {
        get => _xmax;
        set
        {
            if (SetField(ref _xmax, value)) ValidateX();
        }
    }

    public string Ymin
    {
        get => _ymin;
        set
        {
            if (SetField(ref _ymin, value)) ValidateY();
        }
    }

    public string Ymax
    {
        get => _ymax;
        set
        {
            if (SetField(ref _ymax, value)) ValidateY();
        }
    }

    #endregion

    #region ErrorMessages

    private string _xError = string.Empty;
    private string _yError = string.Empty;

    public string XError
    {
        get => _xError;
        private set => SetField(ref _xError, value);
    }

    public string YError
    {
        get => _yError;
        private set => SetField(ref _yError, value);
    }

    #endregion

    #region Validators

    private void ValidateX()
    {
        if (!double.TryParse(Xmin, out var xmin) ||
            !double.TryParse(Xmax, out var xmax))
            XError = "Enter numeric Xmin/Xmax.";
        else if (xmin >= xmax)
            XError = "Xmin must be less than Xmax.";
        else if (xmin < -1000 || xmax > 1000)
            XError = "X range must be between -1000 and 1000.";
        else
            XError = string.Empty;
    }

    private void ValidateY()
    {
        if (!double.TryParse(Ymin, out var ymin) ||
            !double.TryParse(Ymax, out var ymax))
            YError = "Enter numeric Ymin/Ymax.";
        else if (ymin >= ymax)
            YError = "Ymin must be less than Ymax.";
        else if (ymin < -1000 || ymax > 1000)
            YError = "Y range must be between -1000 and 1000.";
        else
            YError = string.Empty;
    }

    #endregion

    #region Helpers

    public bool TryGetX(out double xmin, out double xmax)
    {
        var ok = double.TryParse(Xmin, out xmin);
        ok &= double.TryParse(Xmax, out xmax);
        ok &= xmin < xmax && xmin >= -1000 && xmax <= 1000;
        return ok;
    }

    public bool TryGetY(out double ymin, out double ymax)
    {
        var ok = double.TryParse(Ymin, out ymin);
        ok &= double.TryParse(Ymax, out ymax);
        ok &= ymin < ymax && ymin >= -1000 && ymax <= 1000;
        return ok;
    }

    #endregion
}