using KeeperComparer.Validators;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class LandlineNumberValidatorTests
    {
        private readonly LandlineNumberValidator _validator = new LandlineNumberValidator();

        [Theory]
        [InlineData("01632 960000")]       // 01 prefix
        [InlineData("020 7946 0000")]      // 02 prefix
        [InlineData("03069 990000")]       // 03 prefix
        [InlineData("+44 1632 960123")]    // International format
        [InlineData("(01632) 960 123")]    // Brackets and spaces
        public void IsValid_ShouldReturnTrue_ForValidLandlineNumbers(string number)
        {
            Assert.True(_validator.IsValid(number));
        }

        [Theory]
        [InlineData("07700 900000")]       // Mobile prefix
        [InlineData("01632 96000")]        // Too short
        [InlineData("not a number")]
        [InlineData("01632 960000x")]      // Invalid characters
        [InlineData(null)]
        public void IsValid_ShouldReturnFalse_ForInvalidOrNonLandlineNumbers(string number)
        {
            Assert.False(_validator.IsValid(number));
        }
    }
}