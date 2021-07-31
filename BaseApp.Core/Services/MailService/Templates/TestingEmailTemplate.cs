using System.Threading.Tasks;

namespace BaseApp.Core.Services.MailService.Templates
{
    internal class TestingEmailTemplate : MailService, IMailService 
    {
        public TestingEmailTemplate(MailArgs args) : base(args)
        {
        }

        public async Task SendEmail()
        {
            await Send(GetEmailTemplate());
        }
        
        private static string GetEmailTemplate()
        {
            return "Si estas viendo esto el test paso con exitó";
        }
    }
}