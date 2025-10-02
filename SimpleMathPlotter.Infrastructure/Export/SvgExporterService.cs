using System.Globalization;
using SimpleMathPlotter.Core.Interfaces;

namespace SimpleMathPlotter.Infrastructure.Export;

/// <summary>
/// Class that exports function data to an SVG file.
/// </summary>
public class SvgExporterService : IExportService
{
    /// <inheritdoc cref="IExportService"/>
    public void Export(IEnumerable<(double x, double y)> points, string path,
        double ymin, double ymax)
    {
        var pts = points.ToList();
        if (pts.Count < 2) return;

        const double width = 1000;
        const double height = 600;
        var xmin = pts.Min(p => p.x);
        var xmax = pts.Max(p => p.x);
        var yMin = ymin;
        var yMax = ymax;

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        using var sw = new StreamWriter(path);

        sw.WriteLine(
            $"<svg xmlns='http://www.w3.org/2000/svg' width='{width}' height='{height}'>");

        // Axes (simple)
        sw.WriteLine(
            $"<line x1='0' y1='{height / 2}' x2='{width}' y2='{height / 2}' stroke='lightgray'/>");
        sw.WriteLine(
            $"<line x1='{width / 2}' y1='0' x2='{width / 2}' y2='{height}' stroke='lightgray'/>");

        var poly = string.Join(" ", pts.Select(MapPoint));
        sw.WriteLine(
            $"<polyline fill='none' stroke='black' stroke-width='1' points='{poly}' />");
        sw.WriteLine("</svg>");
        return;

        string MapPoint((double x, double y) p)
        {
            var sx = (p.x - xmin) / (xmax - xmin) * width;
            var sy = height - (p.y - yMin) / (yMax - yMin) * height;
            return
                $"{sx.ToString(CultureInfo.InvariantCulture)},{sy.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}