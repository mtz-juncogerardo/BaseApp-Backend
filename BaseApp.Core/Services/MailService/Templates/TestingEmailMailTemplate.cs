namespace BaseApp.Core.Services.MailService.Templates
{
    internal class TestingEmailMailTemplate : MailService 
    {
        public TestingEmailMailTemplate(MailArgs args) : base(args)
        {
        }

        protected override string GetEmailTemplate()
        {
            return "Si estas viendo esto el test paso con exitó";
        }
    }
}