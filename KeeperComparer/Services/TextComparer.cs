using KeeperComparer.Interfaces;
using F23.StringSimilarity;

namespace KeeperComparer.Services
{
    public class TextComparer : ITextComparer
    {
        public bool AreEqual(string a, string b)
        {
            var normalizedA = Normalize(a);
            var normalizedB = Normalize(b);
            if (string.IsNullOrEmpty(normalizedA) || string.IsNullOrEmpty(normalizedB))
                return false;
            return normalizedA == normalizedB;
        }

        public bool AreSimilar(string a, string b, double threshold = 0.85)
        {
            var score = SimilarityScore(a, b);
            return score >= threshold;
        }

        public double SimilarityScore(string a, string b)
        {
            var normalizedA = Normalize(a);
            var normalizedB = Normalize(b);
            if (string.IsNullOrEmpty(normalizedA) || string.IsNullOrEmpty(normalizedB))
                return 0.0;
            var jw = new JaroWinkler();
            return jw.Similarity(normalizedA, normalizedB);
        }

        private string Normalize(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;
            return name.Trim().ToLowerInvariant();
        }
    }
}
