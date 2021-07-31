namespace BaseApp.InjectionServices
{
    public interface IConfigurationService
    {
        public string SendGridApiKey { get; set; }
        public string JwtSecret { get; set; }
        public string DbConnectionString { get; set; }
        public string ClientUrl { get; set; }
    }
}