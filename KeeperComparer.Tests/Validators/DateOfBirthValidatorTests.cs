using KeeperComparer.Validators;
using System;
using Xunit;

namespace KeeperComparer.Tests.Validators
{
    public class DateOfBirthValidatorTests
    {
        private readonly DateOfBirthValidator _validator = new DateOfBirthValidator();

        [Fact]
        public void IsValid_ShouldReturnTrue_ForValidPastDate()
        {
            Assert.True(_validator.IsValid(new DateTime(1990, 1, 1)));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_ForFutureDate()
        {
            Assert.False(_validator.IsValid(DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_ForTodaysDate()
        {
            Assert.False(_validator.IsValid(DateTime.UtcNow.Date));
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_ForDefaultDate()
        {
            Assert.False(_validator.IsValid(default(DateTime)));
        }
    }
}