using KeeperComparer.Services;

namespace KeeperComparer.Tests.Services
{
    public class MobileNumberComparerTests
    {
        private readonly MobileNumberComparer _comparer = new MobileNumberComparer();

        [Theory]
        // Basic equivalence
        [InlineData("07700900123", "07700900123")]
        // International formats
        [InlineData("07700 900123", "+44 7700 900123")]
        [InlineData("07700900123", "00447700900123")]

        [InlineData("07911 123 456", "07911123456")]
        [InlineData(" 07700 900123 ", "07700900123")]     // leading/trailing spaces
        [InlineData("07700-900-123", "07700900123")]      // dashes
        [InlineData("07700.900.123", "07700900123")]      // dots
        [InlineData("(07700) 900123", "07700900123")]     // brackets
        [InlineData("0 7 7 0 0 9 0 0 1 2 3", "07700900123")] // spaced out digits
        // Isle of Man number
        [InlineData("07624900123", "07624900123")]

        [InlineData("07700 900123x", "07700900123")]      // trailing letters
        [InlineData("a07700900123", "07700900123")]       // leading letters
        public void AreEqual_ShouldReturnTrue_ForEquivalentNumbers(string a, string b)
        {
            // Act
            var result = _comparer.AreEqual(a, b);

            // Assert
            Assert.True(result);
        }


        [Theory]
        // Different valid numbers
        [InlineData("07700900123", "07700900124")]
        // Invalid prefixes or types
        [InlineData("08001234567", "07700900123")]       // freephone
        [InlineData("01234567890", "07700900123")]       // landline
        // Malformed numbers
        [InlineData("7700900123", "07700900123")]        // missing leading 0
        [InlineData("123", "07700900123")]               // too short
        [InlineData("077009001234", "07700900123")]      // too long
        // Null or empty strings
        [InlineData("", "07700900123")]                  // empty string
        [InlineData(null, "07700900123")]                // null value
        public void AreEqual_ShouldReturnFalse_ForDifferentOrInvalidNumbers(string a, string b)
        {
            var result = _comparer.AreEqual(a, b);

            Assert.False(result);
        }
    }
}