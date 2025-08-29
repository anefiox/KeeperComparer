using KeeperComparer.Services;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class LandlineNumberComparerTests
    {
        private readonly LandlineNumberComparer _comparer = new LandlineNumberComparer();

        [Theory]
        [InlineData("01632960123", "01632960123")]
        [InlineData("020 7946 0123", "+44 20 7946 0123")]
        [InlineData("(01632) 960-123", "01632960123")]
        public void AreEqual_ShouldReturnTrue_ForEquivalentNumbers(string a, string b)
        {
            Assert.True(_comparer.AreEqual(a, b));
        }

        [Theory]
        [InlineData("01632960123", "01632960124")] 
        [InlineData("07700900123", "01632960123")]
        [InlineData(null, "01632960123")]
        public void AreEqual_ShouldReturnFalse_ForDifferentNumbers(string a, string b)
        {
            Assert.False(_comparer.AreEqual(a, b));
        }
    }
}