using KeeperComparer.Services.Base;

namespace KeeperComparer.Services
{
    public class MobileNumberComparer : NumberComparerBase
    {
        public override bool AreEqual(string a, string b)
        {
            var normalizedA = Normalizer.PermissiveNormalize(a);
            var normalizedB = Normalizer.PermissiveNormalize(b);

            if (string.IsNullOrEmpty(normalizedA) || string.IsNullOrEmpty(normalizedB))
                return false;

            return normalizedA == normalizedB;
        }
    }
}