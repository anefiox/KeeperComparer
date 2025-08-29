using KeeperComparer.Interfaces;
using System.Net.Mail;

namespace KeeperComparer.Validators
{
    public class EmailValidator : IEmailValidator
    {
        public bool IsValid(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mail = new MailAddress(email);

                if (mail.Host.EndsWith("."))
                {
                    return false;
                }

                if (!mail.Host.Contains("."))
                {
                    return false;
                }

                if (mail.Host.StartsWith("."))
                {
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}