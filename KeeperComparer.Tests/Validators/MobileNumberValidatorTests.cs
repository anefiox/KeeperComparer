using KeeperComparer.Validators;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class MobileNumberValidatorTests
    {
        private readonly MobileNumberValidator _validator = new MobileNumberValidator();

        [Theory]
        [InlineData("07700 900000")]
        [InlineData("+44 7700 900000")]
        [InlineData("07911 123456")]
        public void IsValid_ShouldReturnTrue_ForValidMobileNumbers(string number)
        {
            Assert.True(_validator.IsValid(number));
        }

        [Theory]
        [InlineData("01632 960000")] 
        [InlineData("07700 90000")] 
        [InlineData("not a number")]
        [InlineData(null)]
        public void IsValid_ShouldReturnFalse_ForInvalidMobileNumbers(string number)
        {
            Assert.False(_validator.IsValid(number));
        }
    }
}