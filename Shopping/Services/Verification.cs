using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Shopping.Services
{
    public class Verification
    {
        internal static bool IsValidEmail(string email)
        {
            if (email.Any())
            {
                try
                {
                    MailAddress mailAddress = new(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$";

            Regex regex = new(pattern);

            return regex.IsMatch(password);
        }
    }
}
