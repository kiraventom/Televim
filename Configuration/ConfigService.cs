using System.Text.Json;
using System.Text.Json.Serialization;

namespace Televim.Configuration;

public record struct InvalidJsonEvent(JsonException ex);
public record struct ConfigReadFailEvent(Exception ex);
public record struct ConfigWriteFailEvent(Exception ex);

public interface IConfigService
{
    Task<Config> GetCurrent();
}

public class ConfigService(IEventService eventService, Paths paths) : IConfigService
{
    private const string CONFIG_FILENAME = "config.json";
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions() 
    {
        AllowTrailingCommas = true,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private string ConfigFilePath { get; } = Path.Combine(paths.ConfigDir, CONFIG_FILENAME);

    private Config CurrentConfig { get; set; }
    private DateTime LastReadTime { get; set; }

    public async Task<Config> GetCurrent()
    {
        if (CurrentConfig is null || DidConfigUpdate())
            (CurrentConfig, LastReadTime) = await ReadFromFile();

        return CurrentConfig;
    }

    private bool DidConfigUpdate()
    {
        var lastWrite = File.GetLastWriteTimeUtc(ConfigFilePath);
        return lastWrite > LastReadTime;
    }

    private async Task<(Config, DateTime)> ReadFromFile()
    {
        Config config = CurrentConfig ?? Config.Default;

        if (!File.Exists(ConfigFilePath))
            await WriteToFile(config);

        try
        {
            using var file = File.OpenRead(ConfigFilePath);
            config = await JsonSerializer.DeserializeAsync<Config>(file, Options);
        }
        catch (JsonException ex)
        {
            await eventService.Raise(new InvalidJsonEvent(ex));
        }
        catch (Exception ex)
        {
            await eventService.Raise(new ConfigReadFailEvent(ex));
        }

        return (config, File.GetLastWriteTimeUtc(ConfigFilePath));
    }

    private async Task WriteToFile(Config config)
    {
        try
        {
            using var file = File.Create(ConfigFilePath);
            await JsonSerializer.SerializeAsync(file, config, Options);
        }
        catch (Exception ex)
        {
            await eventService.Raise(new ConfigWriteFailEvent(ex));
        }
    }
}
