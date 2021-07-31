namespace BaseApp.Core.Services.MailService
{
    public class MailArgs
    {
        public string Subject { get; set; }
        public string ReciverEmail { get; set; }
        public string SendGridApiKey { get; set; }
        public string Link { get; set; }
    }
}