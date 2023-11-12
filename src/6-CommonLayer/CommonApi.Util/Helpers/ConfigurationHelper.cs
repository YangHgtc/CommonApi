using Microsoft.Extensions.Configuration;

namespace CommonApi.Util.Helpers
{
    /// <summary>
    /// 配置
    /// </summary>
    public static class ConfigurationHelper
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(() => new ConfigurationBuilder()
#pragma warning restore IDE1006 // Naming Styles
                               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                               .AddJsonFile("appsettings.json")
                               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                               .Build());

        /// <summary>
        /// 配置
        /// </summary>
        public static IConfiguration Instance
        {
            get { return _configuration.Value; }
        }
    }
}
