using SimpleMathPlotter.Core.Interfaces;
using SimpleMathPlotter.Core.Models;

namespace SimpleMathPlotter.Core.Services;

/// <summary>
/// Class that evaluates mathematical functions over a specified range.
/// </summary>
public class FunctionEngine : IFunctionEngine
{
    /// <inheritdoc cref="IFunctionEngine"/>
    public IEnumerable<(double x, double y)> Evaluate(FunctionType type,
        FunctionParameters parameters,
        double xmin, double xmax, int steps = 1000)
    {
        if (steps <= 0) yield break;

        var dx = (xmax - xmin) / steps;

        for (var i = 0; i <= steps; i++)
        {
            var x = xmin + i * dx;
            var arg = parameters.Frequency * x + parameters.Phase;
            var y = CalculateY(type, parameters, arg);

            yield return (x, y);
        }
    }

    private static double CalculateY(FunctionType type,
        FunctionParameters parameters, double arg)
    {
        return type switch
        {
            FunctionType.Sin => parameters.Amplitude * Math.Sin(arg) +
                                parameters.Offset,
            FunctionType.Cos => parameters.Amplitude * Math.Cos(arg) +
                                parameters.Offset,
            FunctionType.Sinc => parameters.Amplitude * Sinc(arg) +
                                 parameters.Offset,
            _ => 0d
        };
    }

    private static double Sinc(double x) =>
        Math.Abs(x) < 1e-8 ? 1.0 : Math.Sin(x) / x;
}