namespace Televim.Configuration;

public interface IConfigService
{
    Task<Config> GetCurrent();
}

