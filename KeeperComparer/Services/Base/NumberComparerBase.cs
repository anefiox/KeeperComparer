using KeeperComparer.Interfaces;
using KeeperComparer.Services.Utility;

namespace KeeperComparer.Services.Base
{
    public abstract class NumberComparerBase : INumberComparer
    {
        protected readonly NumberNormalizer Normalizer = new NumberNormalizer();

        public abstract bool AreEqual(string a, string b);
    }
}