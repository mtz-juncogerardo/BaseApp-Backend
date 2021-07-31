using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BaseApp.Core.Services.MailService
{
    internal class MailService
    {
        private readonly string _receiverEmail;
        private readonly string _sendGridApiKey;
        private readonly string _subject;
        private const string MailSender = "gera@hackerkat.xyz";
        private const string SenderName = "Mr. Cat";

        protected MailService(MailArgs args)
        {
            _receiverEmail = args.ReciverEmail;
            _sendGridApiKey = args.SendGridApiKey;
            _subject = args.Subject;
        }
        
        protected async Task Send(string template)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(MailSender, SenderName);
            var to = new EmailAddress(_receiverEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, _subject, "", template);
            await client.SendEmailAsync(msg);
        }
    }
}