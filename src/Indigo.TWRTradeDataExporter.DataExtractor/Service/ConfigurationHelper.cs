using System.Configuration;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Service
{
    internal static class ConfigurationHelper
    {
        internal static string GetConnectionString(string key)
        {
            return ConfigurationHelper.GetConnectionString(key, true, string.Empty);
        }

        internal static string GetConnectionString(string key, bool throwExceptionIfMissing, string defaultValue)
        {
            var cs = ConfigurationManager.ConnectionStrings[key];

            if (cs != null)
            {
                var output = cs.ConnectionString;

                if (!string.IsNullOrWhiteSpace(output))
                {
                    return output;
                }
            }

            if (throwExceptionIfMissing)
            {
                throw new ConfigurationErrorsException($"Missing connection string {key}");
            }

            return defaultValue;
        }

        

        internal static string GetAppConfig(string key, bool throwExceptionIfMissing, string defaultValue)
        {
            var output = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrWhiteSpace(output))
            {
                return output;
            }

            if (throwExceptionIfMissing)
            {
                throw new ConfigurationErrorsException($"Missing app config value {key}");
            }

            return defaultValue;
        }
    }
}
