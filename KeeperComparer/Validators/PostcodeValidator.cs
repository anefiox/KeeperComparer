using KeeperComparer.Interfaces;
using System.Text.RegularExpressions;

namespace KeeperComparer.Validators
{
    public class PostcodeValidator : IPostcodeValidator
    {
        private static readonly Regex UkPostcodeRegex = new(
            @"^(GIR ?0AA|(?:(?:[A-PR-UWYZ][0-9][0-9]?|[A-PR-UWYZ][A-HK-Y][0-9][0-9]?|[A-PR-UWYZ][0-9][A-HJKPSTUW]?|[A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRV-Y])) ?[0-9][ABD-HJLNP-UW-Z]{2})$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public bool IsValid(string? postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
                return false;

            return UkPostcodeRegex.IsMatch(postcode.Trim());
        }
    }
}