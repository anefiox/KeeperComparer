using KeeperComparer.Interfaces;
using KeeperComparer.Services.Base;

namespace KeeperComparer.Validators
{
    public class LandlineNumberValidator : NumberComparerBase, ILandlineNumberValidator
    {
        public bool IsValid(string? number)
        {
            var normalized = Normalizer.StrictNormalize(number);
            if (normalized is null || normalized.Length != 11)
                return false;

            return normalized.StartsWith("01") ||
                   normalized.StartsWith("02") ||
                   normalized.StartsWith("03");
        }

        public override bool AreEqual(string a, string b) => throw new NotImplementedException();
    }
}