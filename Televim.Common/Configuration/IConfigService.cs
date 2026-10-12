namespace Televim.Common.Configuration;

public interface IConfigService
{
    Task<Config> GetCurrent();
}

