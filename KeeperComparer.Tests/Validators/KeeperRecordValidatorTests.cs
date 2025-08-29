using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Validators;
using Moq;
using System;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class KeeperRecordValidatorTests
    {
        private readonly Mock<IEmailValidator> _mockEmailValidator;
        private readonly Mock<IMobileNumberValidator> _mockMobileValidator;
        private readonly Mock<ILandlineNumberValidator> _mockLandlineValidator;
        private readonly Mock<IValidator<DateTime>> _mockDobValidator;
        private readonly Mock<IPostcodeValidator> _mockPostcodeValidator;

        private readonly KeeperRecordValidator _validator;

        public KeeperRecordValidatorTests()
        {
            _mockEmailValidator = new Mock<IEmailValidator>();
            _mockMobileValidator = new Mock<IMobileNumberValidator>();
            _mockLandlineValidator = new Mock<ILandlineNumberValidator>();
            _mockDobValidator = new Mock<IValidator<DateTime>>();
            _mockPostcodeValidator = new Mock<IPostcodeValidator>();

            _validator = new KeeperRecordValidator(
                _mockEmailValidator.Object,
                _mockMobileValidator.Object,
                _mockLandlineValidator.Object,
                _mockDobValidator.Object,
                _mockPostcodeValidator.Object
            );
        }

        private KeeperRecord CreateValidTestRecord()
        {
            return new KeeperRecord
            {
                FullName = "John Smith",
                EmailAddress = "test@example.com",
                MobileNumber = "07700900123",
                LandlineNumber = "01632960123",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = new Address { Postcode = "SW1A 0AA" }
            };
        }

        private void SetupAllMocksToSucceed()
        {
            _mockEmailValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockMobileValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockLandlineValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
            _mockDobValidator.Setup(v => v.IsValid(It.IsAny<DateTime>())).Returns(true);
            _mockPostcodeValidator.Setup(v => v.IsValid(It.IsAny<string>())).Returns(true);
        }

        [Fact]
        public void Validate_WithCompletelyValidRecord_ReturnsTrueAndNoErrors()
        {
            var record = CreateValidTestRecord();
            SetupAllMocksToSucceed();

            var (isValid, errors) = _validator.Validate(record);

            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void Validate_WithInvalidEmail_ReturnsFalseAndEmailError()
        {
            var record = CreateValidTestRecord();
            SetupAllMocksToSucceed();
            _mockEmailValidator.Setup(v => v.IsValid(record.EmailAddress)).Returns(false); 

            var (isValid, errors) = _validator.Validate(record);

            Assert.False(isValid);
            Assert.Single(errors);
            Assert.Contains("Email Address", errors[0]);
        }

        [Fact]
        public void Validate_WithInvalidMobileNumber_ReturnsFalseAndMobileError()
        {
            var record = CreateValidTestRecord();
            SetupAllMocksToSucceed();
            _mockMobileValidator.Setup(v => v.IsValid(record.MobileNumber)).Returns(false);

            var (isValid, errors) = _validator.Validate(record);

            Assert.False(isValid);
            Assert.Single(errors);
            Assert.Contains("Mobile Number", errors[0]);
        }

        [Fact]
        public void Validate_WithMissingFullName_ReturnsFalseAndNameError()
        {
            var record = CreateValidTestRecord();
            record.FullName = " ";
            SetupAllMocksToSucceed(); 

            var (isValid, errors) = _validator.Validate(record);

            Assert.False(isValid);
            Assert.Single(errors); 
            Assert.Contains("Full Name", errors[0]);
        }

        [Fact]
        public void Validate_WithMultipleErrors_ReturnsFalseAndAllErrors()
        {
            var record = CreateValidTestRecord();
            SetupAllMocksToSucceed();
            _mockEmailValidator.Setup(v => v.IsValid(record.EmailAddress)).Returns(false);
            _mockMobileValidator.Setup(v => v.IsValid(record.MobileNumber)).Returns(false);

            var (isValid, errors) = _validator.Validate(record);

            Assert.False(isValid);
            Assert.Equal(2, errors.Count);
            Assert.Contains(errors, e => e.Contains("Email"));
            Assert.Contains(errors, e => e.Contains("Mobile"));
        }
    }
}