using KeeperComparer.Services;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class EmailComparerTests
    {
        private readonly EmailComparer _comparer = new EmailComparer();

        [Theory]
        [InlineData("test@example.com", "test@example.com")]
        [InlineData("TEST@EXAMPLE.COM", "test@example.com")]
        [InlineData("  test@example.com  ", "test@example.com")] 
        public void AreEqual_ShouldReturnTrue_ForEquivalentEmails(string a, string b)
        {
            Assert.True(_comparer.AreEqual(a, b));
        }

        [Theory]
        [InlineData("test1@example.com", "test2@example.com")]
        [InlineData("test@example.com", null)]
        [InlineData("test@example.com", "")]
        public void AreEqual_ShouldReturnFalse_ForDifferentOrNullEmails(string a, string b)
        {
            Assert.False(_comparer.AreEqual(a, b));
        }
    }
}