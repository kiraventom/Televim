namespace Televim.Configuration;

public record Config
{
    public static Config Default { get; } = new Config()
    {
        ApiId = 123456789,
        ApiHash = Guid.Empty.ToString("N"),
    };

    public required long ApiId { get; init; }
    public required string ApiHash { get; init; }

    public string ProxyAddress { get; init; }
}

