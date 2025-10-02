using SimpleMathPlotter.Core.Models;

namespace SimpleMathPlotter.Core.Interfaces;

/// <summary>
/// Contract for a service that persists function data to a file.
/// </summary>
public interface IPersistenceService
{
    /// <summary>
    /// Saves the function data to a file.
    /// </summary>
    /// <param name="type">The function type.</param>
    /// <param name="parameters">The function parameters.</param>
    /// <param name="xmin">The minium function argument value.</param>
    /// <param name="xmax">The maximum function argument value.</param>
    /// <param name="ymin">The minium function value.</param>
    /// <param name="ymax">The maximum function value.</param>
    void Save(
        FunctionType type,
        FunctionParameters parameters,
        double xmin,
        double xmax,
        double ymin,
        double ymax);

    /// <summary>
    /// Loads the function data from a file.
    /// </summary>
    /// <returns>The function data or null if no settings were loaded</returns>
    (FunctionType type, FunctionParameters parameters, double xmin, double
        xmax, double ymin, double ymax)? Load();
}