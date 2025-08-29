using KeeperComparer.Interfaces;

namespace KeeperComparer.Services
{
    public class PostcodeComparer : IPostcodeComparer
    {
        public bool AreEqual(string? a, string? b)
        {
            var normA = Normalize(a);
            var normB = Normalize(b);

            if (string.IsNullOrEmpty(normA) || string.IsNullOrEmpty(normB))
            {
                return false;
            }

            return normA == normB;
        }

        public string Normalize(string? postcode)
        {
            return string.IsNullOrWhiteSpace(postcode)
                ? string.Empty
                : postcode.Replace(" ", string.Empty).ToUpperInvariant();
        }
    }
}