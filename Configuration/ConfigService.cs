using System.Text.Json;
using System.Text.Json.Serialization;

namespace Televim.Configuration;

public record struct InvalidJsonEvent(JsonException ex);
public record struct ConfigReadFailEvent(Exception ex);
public record struct ConfigWriteFailEvent(Exception ex);
public record struct DefaultConfigGeneratedEvent(string ConfigFilePath);
public record struct DefaultConfigDetectedEvent(string ConfigFilePath, params string[] PropsToChange);

public class ConfigService : IConfigService
{
    private const string CONFIG_FILENAME = "config.json";
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions() 
    {
        AllowTrailingCommas = true,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly IEventService _eventService;
    private readonly string _configFilePath;
    private readonly Config _defaultConfig;

    private Config CurrentConfig { get; set; }
    private DateTime LastReadTime { get; set; }

    public ConfigService(IEventService eventService, Paths paths)
    {
        _eventService = eventService;
        _configFilePath = Path.Combine(paths.ConfigDir, CONFIG_FILENAME);
        _defaultConfig = DefaultConfigGenerator.Generate(paths);
    }

    public async Task<Config> GetCurrent()
    {
        if (CurrentConfig is null || DidConfigUpdate())
            (CurrentConfig, LastReadTime) = await ReadFromFile();

        return CurrentConfig;
    }

    private bool DidConfigUpdate()
    {
        var lastWrite = File.GetLastWriteTimeUtc(_configFilePath);
        return lastWrite > LastReadTime;
    }

    private async Task<(Config, DateTime)> ReadFromFile()
    {
        Config config = CurrentConfig;

        if (!File.Exists(_configFilePath))
            await WriteToFile(config);

        try
        {
            using var file = File.OpenRead(_configFilePath);
            config = await JsonSerializer.DeserializeAsync<Config>(file, Options);
        }
        catch (JsonException ex)
        {
            await _eventService.Raise(new InvalidJsonEvent(ex));
        }
        catch (Exception ex)
        {
            await _eventService.Raise(new ConfigReadFailEvent(ex));
        }

        if (config.IsDefault())
        {
            await _eventService.Raise(new DefaultConfigDetectedEvent(_configFilePath, nameof(Config.TDLib.ApiId), nameof(Config.TDLib.ApiHash)));
        }

        return (config, File.GetLastWriteTimeUtc(_configFilePath));
    }

    private async Task WriteToFile(Config config)
    {
        try
        {
            using var file = File.Create(_configFilePath);

            if (config is null)
            {
                config = _defaultConfig;
                await _eventService.Raise(new DefaultConfigGeneratedEvent(_configFilePath));
            }

            await JsonSerializer.SerializeAsync(file, config, Options);
        }
        catch (Exception ex)
        {
            await _eventService.Raise(new ConfigWriteFailEvent(ex));
        }
    }
}
