using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Validators;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class MockPostcodeValidator : IPostcodeValidator
    {
        private bool _isValid = true;
        public void SetIsValid(bool isValid) => _isValid = isValid;
        public bool IsValid(string? value) => _isValid;
    }

    public class AddressValidatorTests
    {
        private readonly MockPostcodeValidator _mockPostcodeValidator;
        private readonly AddressValidator _addressValidator;

        public AddressValidatorTests()
        {
            _mockPostcodeValidator = new MockPostcodeValidator();
            _addressValidator = new AddressValidator(_mockPostcodeValidator);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllFieldsAreValid()
        {
            var address = new Address
            {
                Line1 = "123 Fake Street",
                Postcode = "VALIDPOSTCODE"
            };

            _mockPostcodeValidator.SetIsValid(true);

            var result = _addressValidator.IsValid(address);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenPostcodeIsInvalid()
        {
            var address = new Address
            {
                Line1 = "123 Fake Street",
                Postcode = "INVALIDPOSTCODE"
            };

            _mockPostcodeValidator.SetIsValid(false);

            var result = _addressValidator.IsValid(address);

            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_ShouldReturnFalse_WhenLine1IsMissing(string line1)
        {
            var address = new Address
            {
                Line1 = line1,
                Postcode = "VALIDPOSTCODE"
            };
            _mockPostcodeValidator.SetIsValid(true);

            var result = _addressValidator.IsValid(address);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAddressIsNull()
        {
            var result = _addressValidator.IsValid(null);

            Assert.False(result);
        }
    }
}