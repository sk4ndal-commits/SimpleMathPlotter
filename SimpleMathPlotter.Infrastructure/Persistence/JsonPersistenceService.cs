using System.Text.Json;
using SimpleMathPlotter.Core.Interfaces;
using SimpleMathPlotter.Core.Models;

namespace SimpleMathPlotter.Infrastructure.Persistence;

/// <summary>
/// Class responsible for persisting function data to a JSON file.
/// </summary>
public class JsonPersistenceService : IPersistenceService
{
    private static string SettingsPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .ApplicationData),
            "SimpleMathPlotter", "smp_settings.json");

    /// <inheritdoc cref="IPersistenceService"/>
    public void Save(FunctionType type, FunctionParameters parameters,
        double xmin,
        double xmax, double ymin, double ymax)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            using var fs = File.Create(SettingsPath);
            using var writer = new Utf8JsonWriter(fs,
                new JsonWriterOptions { Indented = true });

            writer.WriteStartObject();
            writer.WriteString("type", type.ToString());
            writer.WritePropertyName("parameters");
            writer.WriteStartObject();
            writer.WriteNumber(nameof(parameters.Amplitude),
                parameters.Amplitude);
            writer.WriteNumber(nameof(parameters.Frequency),
                parameters.Frequency);
            writer.WriteNumber(nameof(parameters.Phase),
                parameters.Phase);
            writer.WriteNumber(nameof(parameters.Offset),
                parameters.Offset);
            writer.WriteEndObject();
            
            writer.WriteNumber("xmin", xmin);
            writer.WriteNumber("xmax", xmax);
            writer.WriteNumber("ymin", ymin);
            writer.WriteNumber("ymax", ymax);
            writer.WriteEndObject();
        }
        catch
        {
        }
    }

    /// <inheritdoc cref="IPersistenceService"/>
    public (FunctionType type, FunctionParameters parameters, double xmin,
        double xmax,
        double ymin, double ymax)? Load()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return null;

            using var fs = File.OpenRead(SettingsPath);
            var doc = JsonDocument.Parse(fs);
            var root = doc.RootElement;

            var type =
                Enum.Parse<FunctionType>(root.GetProperty("type").GetString()!);
            var p = root.GetProperty("parameters");
            var parameters = new FunctionParameters
            {
                Amplitude = p.GetProperty("Amplitude").GetDouble(),
                Frequency = p.GetProperty("Frequency").GetDouble(),
                Phase = p.GetProperty("Phase").GetDouble(),
                Offset = p.GetProperty("Offset").GetDouble(),
            };

            var xmin = root.GetProperty("xmin").GetDouble();
            var xmax = root.GetProperty("xmax").GetDouble();
            var ymin = root.GetProperty("ymin").GetDouble();
            var ymax = root.GetProperty("ymax").GetDouble();

            return (type, parameters, xmin, xmax, ymin, ymax);
        }
        catch
        {
            return null;
        }
    }
}