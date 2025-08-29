using KeeperComparer.Interfaces;
using KeeperComparer.Services.Base;

namespace KeeperComparer.Validators
{
    public class MobileNumberValidator : NumberComparerBase, IMobileNumberValidator
    {
        private static readonly string[] ValidMobilePrefixes = new[]
        {
            "071", "072", "073", "074", "075", "07624", "077", "078", "079"
        };

        public bool IsValid(string? number)
        {
            var normalized = Normalizer.StrictNormalize(number);
            if (normalized is null || normalized.Length != 11) return false;
            if (!normalized.StartsWith("07")) return false;

            return ValidMobilePrefixes.Any(prefix => normalized.StartsWith(prefix));
        }
        public override bool AreEqual(string a, string b) => throw new NotImplementedException();
    }
}