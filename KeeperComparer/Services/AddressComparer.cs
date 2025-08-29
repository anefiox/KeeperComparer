using KeeperComparer.Models;
using KeeperComparer.Interfaces;

namespace KeeperComparer.Services
{
    public class AddressComparer : IAddressComparer
    {
        private readonly ITextComparer _textComparer;
        private const double SimilarityThreshold = 0.85;

        public AddressComparer() : this(new TextComparer()) { }
        public AddressComparer(ITextComparer textComparer)
        {
            _textComparer = textComparer;
        }

        public bool AreEqual(Address? a, Address? b)
        {
            if (a == null || b == null)
                return false;
            return _textComparer.AreEqual(a.Line1, b.Line1)
                && _textComparer.AreEqual(a.Line2, b.Line2)
                && _textComparer.AreEqual(a.City, b.City)
                && NormalizePostcode(a.Postcode) == NormalizePostcode(b.Postcode);
        }

        public bool AreSimilar(Address? a, Address? b)
        {
            if (a == null || b == null)
                return false;
            if (NormalizePostcode(a.Postcode) != NormalizePostcode(b.Postcode))
                return false;
            return _textComparer.AreSimilar(a.Line1, b.Line1, SimilarityThreshold)
                && _textComparer.AreSimilar(a.Line2, b.Line2, SimilarityThreshold)
                && _textComparer.AreSimilar(a.City, b.City, SimilarityThreshold);
        }

        private string NormalizePostcode(string? postcode)
        {
            return string.IsNullOrWhiteSpace(postcode) ? string.Empty : postcode.Replace(" ", string.Empty).ToUpperInvariant();
        }
    }
}
