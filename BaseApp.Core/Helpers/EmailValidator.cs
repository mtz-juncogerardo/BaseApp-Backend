using System.Text.RegularExpressions;

namespace BaseApp.Core.Helpers
{
    public static class EmailValidator
    {
        private static Regex ValidEmailRegex => CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        private static bool FormatIsValid(string email)
        {
            var splitEmail = email.Split("@");
            return splitEmail.Length == 2 && splitEmail[0].Length >= 3;
        }
        public static void ValidateEmail(string emailAddress)
        {
            var isValid = ValidEmailRegex.IsMatch(emailAddress);
            if (!isValid || !FormatIsValid(emailAddress))
            {
                CustomException.Throw("El correo ingresado no es valido.", 400);
            }
        }
    }
}