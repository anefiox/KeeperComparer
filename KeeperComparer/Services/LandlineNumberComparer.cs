using KeeperComparer.Services.Base;

namespace KeeperComparer.Services
{
    public class LandlineNumberComparer : NumberComparerBase
    {
        public override bool AreEqual(string a, string b)
        {
            var normalizedA = Normalizer.StrictNormalize(a);
            var normalizedB = Normalizer.StrictNormalize(b);


            if (string.IsNullOrEmpty(normalizedA) || string.IsNullOrEmpty(normalizedB))
                return false;

            return normalizedA == normalizedB;
        }
    }
}

  