using System.Threading.Tasks;

namespace BaseApp.Core.Services.MailService
{
    public interface IMailService
    {
        Task SendEmail();
    }
}