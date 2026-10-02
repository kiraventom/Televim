namespace Televim.Core.Configuration;

public interface IConfigService
{
    Task<Config> GetCurrent();
}

