using System.Text.RegularExpressions;

namespace KeeperComparer.Services.Utility
{
    public class NumberNormalizer
    {
        public string PermissiveNormalize(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return "";

            var digitsOnly = new string(number.Where(char.IsDigit).ToArray());

            if (digitsOnly.StartsWith("0044"))
                return "0" + digitsOnly.Substring(4);
            if (digitsOnly.StartsWith("44"))
                return "0" + digitsOnly.Substring(2);

            return digitsOnly;
        }

        public string? StrictNormalize(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return null;

            var invalidCharRegex = new Regex(@"[^\d\s\+\-\(\)]");
            if (invalidCharRegex.IsMatch(number))
            {
                return null; 
            }

            return PermissiveNormalize(number);
        }
    }
}