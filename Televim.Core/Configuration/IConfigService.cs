namespace Televim.Core.Configuration;

internal interface IConfigService
{
    Task<Config> GetCurrent();
}

