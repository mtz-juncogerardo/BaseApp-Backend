using System.Threading.Tasks;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.MailService.Templates;

namespace BaseApp.Core.Services.MailService
{
    public static class MailFactory
    {
        public static async Task SendMailTemplate(MailTemplateType templateType, MailArgs args)
        {
            switch (templateType)
            {
                case MailTemplateType.ConfirmEmail:
                    await new ConfirmEmailMailTemplate(args).Send();
                    break;
                case MailTemplateType.EmailChange:
                    await new EmailChangeMailTemplate(args).Send();
                    break;
                case MailTemplateType.PasswordRecovery:
                    await new PasswordRecoverMailTemplate(args).Send();
                    break;
                case MailTemplateType.TestingEmail:
                    await new TestingEmailMailTemplate(args).Send();
                    break;
                default:
                    CustomException.Throw("No se encontro un mail template para enviar", 500);
                    break;
            }
        }
    }
}