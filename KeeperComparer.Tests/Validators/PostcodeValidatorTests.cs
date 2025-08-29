using KeeperComparer.Validators;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class UkPostcodeValidatorTests
    {
        private readonly PostcodeValidator _validator;

        public UkPostcodeValidatorTests()
        {
            _validator = new PostcodeValidator();
        }

        [Theory]
        [InlineData("SW1A 0AA")] 
        [InlineData("GIR 0AA")] 
        [InlineData("M1 1AE")]  
        [InlineData("cr26xh")]  
        [InlineData("DN55 1PT")]
        public void IsValid_ShouldReturnTrue_ForValidPostcodes(string validPostcode)
        {
            var result = _validator.IsValid(validPostcode);

            Assert.True(result);
        }

        [Theory]
        [InlineData("NotAPostcode")]
        [InlineData("SW1A 0A")]
        [InlineData("12345")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void IsValid_ShouldReturnFalse_ForInvalidPostcodes(string invalidPostcode)
        {
            var result = _validator.IsValid(invalidPostcode);

            Assert.False(result);
        }
    }
}