using Microsoft.Extensions.Configuration;

namespace BaseApp.InjectionServices
{
    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService(IConfiguration configuration)
        {
            SendGridApiKey = configuration["SendGridApiKey"];
            JwtSecret = configuration["JwtSecret"];
            DbConnectionString = configuration["DbConnectionString"];
            ClientUrl = configuration["ClientUrl"];
        }
        public string SendGridApiKey { get; set; }
        public string JwtSecret { get; set; }
        public string DbConnectionString { get; set; }
        public string ClientUrl { get; set; }
    }
}