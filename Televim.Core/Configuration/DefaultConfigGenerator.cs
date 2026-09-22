using System.Globalization;

namespace Televim.Core.Configuration;

public static class DefaultConfigGenerator
{
    public const int DEFAULT_API_ID = 123456789;
    public static readonly string DEFAULT_API_HASH = Guid.Empty.ToString("N");

    public static Config Generate(Paths paths)
    {
        var tdLibDbPath = Path.Combine(paths.DataDir, "tdlib", "db");
        Directory.CreateDirectory(tdLibDbPath);

        var defaultConfig = new Config()
        {
            TDLib = new TDLibConfig()
            {
                ApiId = DEFAULT_API_ID,
                ApiHash = DEFAULT_API_HASH,
                DatabaseDir = tdLibDbPath,
                UseSecretChats = false,
                SystemLanguageCode = CultureInfo.CurrentCulture.IetfLanguageTag,
                DeviceModel = GetDeviceModel(),
                SystemVersion = Environment.OSVersion.VersionString
            },
        };

        return defaultConfig;
    }

    private static string GetDeviceModel()
    {
        string dmiPath = "/sys/devices/virtual/dmi/id/product_name";
        if (File.Exists(dmiPath))
        {
            try
            {
                string model = File.ReadAllText(dmiPath).Trim();
                if (!string.IsNullOrEmpty(model) && model != "None" && model != "System Product Name")
                    return model;
            }
            catch 
            {}
        }

        string armPath = "/proc/device-tree/model";
        if (File.Exists(armPath))
        {
            try
            {
                string model = File.ReadAllText(armPath).Trim('\0', ' ', '\r', '\n');
                if (!string.IsNullOrEmpty(model))
                    return model;
            }
            catch 
            {}
        }

        return Environment.MachineName;
    }
}

