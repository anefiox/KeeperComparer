using KeeperComparer.Services;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class PostcodeComparerTests
    {
        private readonly PostcodeComparer _comparer;

        public PostcodeComparerTests()
        {
            _comparer = new PostcodeComparer();
        }

        [Theory]
        [InlineData("SW1A 0AA", "sw1a0aa")]
        [InlineData(" M1 1AE ", "m11ae")]
        [InlineData("cr26xh", "CR26XH")]
        public void AreEqual_ShouldReturnTrue_ForEquivalentPostcodes(string a, string b)
        {
            var result = _comparer.AreEqual(a, b);

            Assert.True(result);
        }

        [Fact]
        public void AreEqual_ShouldReturnFalse_ForDifferentPostcodes()
        {
            var a = "SW1A 0AA";
            var b = "SW1A 0AB";

            var result = _comparer.AreEqual(a, b);

            Assert.False(result);
        }

        [Theory]
        [InlineData(null, "SW1A 0AA")]
        [InlineData("SW1A 0AA", "")]
        [InlineData(null, null)]
        public void AreEqual_ShouldHandleNullOrEmptyStrings(string a, string b)
        {
            var areEqual = _comparer.AreEqual(a, b);

            Assert.False(areEqual);
        }
    }
}