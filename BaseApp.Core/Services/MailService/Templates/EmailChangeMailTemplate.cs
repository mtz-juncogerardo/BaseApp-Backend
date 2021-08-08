using BaseApp.Core.Helpers;

namespace BaseApp.Core.Services.MailService.Templates
{
    internal class EmailChangeMailTemplate : MailService
    {
        private readonly string _link;
        public EmailChangeMailTemplate(MailArgs args) : base(args)
        {
            if (args.Link == string.Empty)
            {
                CustomException.Throw("No se especifico un link valido para el template", 500);
            }
            _link = args.Link;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>Has solicitado un cambio de correo</h1> <p>Para confirmar este nuevo correo por favor haz click en el siguiente enlace:</p> <a href=\"{_link}\" target=_blank>Click Aquí</a> <br>" +
                   $"<p>Si no puedes acceder al link, copia y pega el siguiente enlace en tu navegador:<p><br>{_link}";
        }
    }
}