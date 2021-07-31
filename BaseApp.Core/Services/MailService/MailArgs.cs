namespace BaseApp.Core.Services.MailService
{
    public class MailArgs
    {
        public string Subject { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public string SendGridApiKey { get; set; } = null!;
        public string Link { get; set; } = null!;
    }
}