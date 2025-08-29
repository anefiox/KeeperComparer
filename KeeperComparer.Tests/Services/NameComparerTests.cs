using KeeperComparer.Interfaces;
using KeeperComparer.Services;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class NameComparerTests
    {
        private readonly MockTextComparer _mockTextComparer;
        private readonly NameComparer _nameComparer;

        public NameComparerTests()
        {
            _mockTextComparer = new MockTextComparer();
            _nameComparer = new NameComparer(_mockTextComparer);
        }

        [Fact]
        public void AreEqual_CallsTextComparerAreEqual()
        {
            _mockTextComparer.AreEqualResult = true;
            Assert.True(_nameComparer.AreEqual("John", "John"));

            _mockTextComparer.AreEqualResult = false;
            Assert.False(_nameComparer.AreEqual("John", "Jane"));
        }

        [Fact]
        public void AreSimilar_CallsTextComparerAreSimilar()
        {
            _mockTextComparer.AreSimilarResult = true;
            Assert.True(_nameComparer.AreSimilar("Jon", "John"));

            _mockTextComparer.AreSimilarResult = false;
            Assert.False(_nameComparer.AreSimilar("Jon", "Jane"));
        }

        [Fact]
        public void SimilarityScore_CallsTextComparerSimilarityScore()
        {
            _mockTextComparer.SimilarityScoreResult = 0.95;
            Assert.Equal(0.95, _nameComparer.SimilarityScore("Jon", "John"));
        }
    }
}