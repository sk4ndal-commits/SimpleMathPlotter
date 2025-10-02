using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

/// <summary>
/// ViewModel for configuring parameters of a mathematical function.
/// </summary>
public class ParameterSettingsViewModel : ViewModelBase
{
    #region Parameters

    private string _amplitude = "1";
    private string _frequency = "1";
    private string _phase = "0";
    private string _offset = "0";

    public string Amplitude
    {
        get => _amplitude;
        set
        {
            if (SetField(ref _amplitude, value)) ValidateAmplitude();
        }
    }

    public string Frequency
    {
        get => _frequency;
        set
        {
            if (SetField(ref _frequency, value)) ValidateFrequency();
        }
    }

    public string Phase
    {
        get => _phase;
        set
        {
            if (SetField(ref _phase, value)) ValidatePhase();
        }
    }

    public string Offset
    {
        get => _offset;
        set
        {
            if (SetField(ref _offset, value)) ValidateOffset();
        }
    }

    #endregion

    #region ErrorMessages

    private string _amplitudeError = string.Empty;
    private string _frequencyError = string.Empty;
    private string _phaseError = string.Empty;
    private string _offsetError = string.Empty;

    public string AmplitudeError
    {
        get => _amplitudeError;
        private set => SetField(ref _amplitudeError, value);
    }

    public string FrequencyError
    {
        get => _frequencyError;
        private set => SetField(ref _frequencyError, value);
    }

    public string PhaseError
    {
        get => _phaseError;
        private set => SetField(ref _phaseError, value);
    }

    public string OffsetError
    {
        get => _offsetError;
        private set => SetField(ref _offsetError, value);
    }

    #endregion

    #region Values

    public double AmplitudeValue =>
        double.TryParse(Amplitude, out var v) ? v : 0;

    public double FrequencyValue =>
        double.TryParse(Frequency, out var v) ? v : 0;

    public double PhaseValue =>
        double.TryParse(Phase, out var v) ? v : 0;

    public double OffsetValue =>
        double.TryParse(Offset, out var v) ? v : 0;

    #endregion

    #region Errors

    public bool HasErrors =>
        !string.IsNullOrEmpty(AmplitudeError) ||
        !string.IsNullOrEmpty(FrequencyError) ||
        !string.IsNullOrEmpty(PhaseError) ||
        !string.IsNullOrEmpty(OffsetError);

    #endregion

    #region Validators

    private void ValidateAmplitude()
    {
        if (!double.TryParse(Amplitude, out var v))
            AmplitudeError = "Enter a numeric amplitude.";
        else if (v is < -100 or > 100)
            AmplitudeError = "Amplitude must be between -100 and 100.";
        else
            AmplitudeError = string.Empty;
    }

    private void ValidateFrequency()
    {
        if (!double.TryParse(Frequency, out var v))
            FrequencyError = "Enter a numeric frequency.";
        else if (v is <= 0 or > 100)
            FrequencyError = "Frequency must be between 0 and 100.";
        else
            FrequencyError = string.Empty;
    }

    private void ValidatePhase()
    {
        if (!double.TryParse(Phase, out var v))
            PhaseError = "Enter a numeric phase.";
        else if (v is < -2 * Math.PI or > 2 * Math.PI)
            PhaseError = "Phase must be between [-2π, 2π].";
        else
            PhaseError = string.Empty;
    }

    private void ValidateOffset()
    {
        if (!double.TryParse(Offset, out var v))
            OffsetError = "Enter a numeric offset.";
        else if (v is < -100 or > 100)
            OffsetError = "Offset must be between -100 and 100.";
        else
            OffsetError = string.Empty;
    }

    #endregion
}