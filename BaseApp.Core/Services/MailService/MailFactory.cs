using System.Threading.Tasks;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.MailService.Templates;

namespace BaseApp.Core.Services.MailService
{
    public static class MailFactory
    {
        public static async Task SendMailTemplate(MailTemplateEnum templateType, MailArgs args)
        {
            switch (templateType)
            {
                case MailTemplateEnum.ConfirmEmail:
                    await new ConfirmEmailTemplate(args).SendEmail();
                    break;
                case MailTemplateEnum.EmailChange:
                    await new EmailChangeTemplate(args).SendEmail();
                    break;
                case MailTemplateEnum.PasswordRecovery:
                    await new PasswordRecoverTemplate(args).SendEmail();
                    break;
                case MailTemplateEnum.TestingEmail:
                    await new TestingEmailTemplate(args).SendEmail();
                    break;
                default:
                    CustomException.Throw("No se espicifico este tipo de template en el switch", 500);
                    break;
            }
        }
    }
}