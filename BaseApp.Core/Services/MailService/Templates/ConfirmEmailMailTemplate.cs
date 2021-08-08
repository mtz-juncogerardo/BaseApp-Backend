using BaseApp.Core.Helpers;

namespace BaseApp.Core.Services.MailService.Templates
{
    internal class ConfirmEmailMailTemplate : MailService
    {
        private readonly string _link;
        public ConfirmEmailMailTemplate(MailArgs args) : base(args)
        {
            if (args.Link == string.Empty)
            {
                CustomException.Throw("No se especifico un link valido para el template", 500);
            }
            _link = args.Link;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>Gracias por registrarte</h1> <p>Para terminar tu registro por favor haz click en el siguiente enlace.</p> <a href=\"{_link}\" target=_blank>Click Aquí</a> <br>" +
                   $"<p>Si no puedes acceder al link, copia y pega el siguiente enlace en tu navegador:<p><br>{_link}";
        }
    }
}