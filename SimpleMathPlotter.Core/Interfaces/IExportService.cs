namespace SimpleMathPlotter.Core.Interfaces;

/// <summary>
/// Contract for a service that exports function data to a file.
/// </summary>
public interface IExportService
{
    
    /// <summary>
    /// Exports the given points to 'path'.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="path"></param>
    /// <param name="ymin"></param>
    /// <param name="ymax"></param>
    void Export(
        IEnumerable<(double x, double y)> points,
        string path,
        double ymin,
        double ymax);
}