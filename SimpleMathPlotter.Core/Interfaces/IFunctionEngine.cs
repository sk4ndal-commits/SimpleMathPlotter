using SimpleMathPlotter.Core.Models;

namespace SimpleMathPlotter.Core.Interfaces;

public interface IFunctionEngine
{
    /// <summary>
    /// Sample the specified function type over the given range and number of steps.
    /// </summary>
    /// <param name="type">The function type.</param>
    /// <param name="parameters">The function parameters.</param>
    /// <param name="xmin">The minium function argument value.</param>
    /// <param name="xmax">The maximum function argument value.</param>
    /// <param name="steps">The number of sample points.</param>
    /// <returns></returns>
    IEnumerable<(double x, double y)> Evaluate(
        FunctionType type,
        FunctionParameters parameters,
        double xmin,
        double xmax,
        int steps = 1000);
}