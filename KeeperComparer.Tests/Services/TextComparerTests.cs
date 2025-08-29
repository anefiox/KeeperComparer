using KeeperComparer.Services;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class TextComparerTests
    {
        private readonly TextComparer _comparer = new TextComparer();

        [Theory]
        [InlineData("John Smith", "john smith")]
        [InlineData("  Some Name  ", "Some Name")]
        [InlineData("Test", "Test")]
        public void AreEqual_ShouldReturnTrue_ForEquivalentText(string a, string b)
        {
            Assert.True(_comparer.AreEqual(a, b));
        }

        [Theory]
        [InlineData("John", "Jane")]
        [InlineData("Test", null)]
        [InlineData("", "Test")]
        public void AreEqual_ShouldReturnFalse_ForDifferentText(string a, string b)
        {
            Assert.False(_comparer.AreEqual(a, b));
        }

        [Fact]
        public void SimilarityScore_ShouldBeHigh_ForSimilarStrings()
        {
            var score = _comparer.SimilarityScore("Jonathan", "Jonathon");
            Assert.True(score > 0.9);
        }

        [Fact]
        public void SimilarityScore_ShouldBeLow_ForDissimilarStrings()
        {
            var score = _comparer.SimilarityScore("Apple", "Banana");
            Assert.True(score < 0.5);
        }

        [Fact]
        public void SimilarityScore_ShouldBeOne_ForIdenticalStrings()
        {
            var score = _comparer.SimilarityScore("  TEST  ", "test");
            Assert.Equal(1.0, score);
        }

        [Fact]
        public void AreSimilar_ShouldReturnTrue_WhenScoreIsAboveThreshold()
        { 
            Assert.True(_comparer.AreSimilar("Martha", "Marhta", 0.85));
        }

        [Fact]
        public void AreSimilar_ShouldReturnFalse_WhenScoreIsBelowThreshold()
        {
            Assert.False(_comparer.AreSimilar("Apple", "Banana", 0.85));
        }
    }
}