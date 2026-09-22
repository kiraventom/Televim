namespace Televim.Core.Configuration;

internal record Config
{
    public required TDLibConfig TDLib { get; init; }
    public ConnectionConfig Connection { get; init; }

    public bool IsDefault()
    {
        return TDLib.ApiId == DefaultConfigGenerator.DEFAULT_API_ID || TDLib.ApiHash == DefaultConfigGenerator.DEFAULT_API_HASH;
    }
}

internal record TDLibConfig
{
    public required int ApiId { get; init; }
    public required string ApiHash { get; init; }
    public string DatabaseDir { get; init; }
    public bool UseMessageDatabase { get; init; }
    public bool UseSecretChats { get; init; }
    public string SystemLanguageCode { get; init; }
    public string DeviceModel { get; init; }
    public string SystemVersion { get; init; }
}

internal record ConnectionConfig
{
    public string ProxyAddress { get; init; }
}
