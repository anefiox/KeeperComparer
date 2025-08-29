using KeeperComparer.Interfaces;

namespace KeeperComparer.Services
{
    public class NameComparer : ITextComparer
    {
        private readonly ITextComparer _textComparer;

        public NameComparer() : this(new TextComparer()) { }

        public NameComparer(ITextComparer textComparer)
        {
            _textComparer = textComparer;
        }

        public bool AreEqual(string a, string b)
            => _textComparer.AreEqual(a, b);

        public bool AreSimilar(string a, string b, double threshold = 0.85)
            => _textComparer.AreSimilar(a, b, threshold);

        public double SimilarityScore(string a, string b)
            => _textComparer.SimilarityScore(a, b);
    }
}
