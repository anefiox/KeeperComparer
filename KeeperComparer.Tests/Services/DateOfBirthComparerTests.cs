using KeeperComparer.Services;
using System;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class DateOfBirthComparerTests
    {
        private readonly DateOfBirthComparer _comparer = new DateOfBirthComparer();

        [Fact]
        public void AreEqual_ShouldReturnTrue_ForSameDate()
        {
            var dateA = new DateTime(1995, 10, 20);
            var dateB = new DateTime(1995, 10, 20);
            Assert.True(_comparer.AreEqual(dateA, dateB));
        }

        [Fact]
        public void AreEqual_ShouldReturnTrue_ForSameDateWithDifferentTimes()
        {
            var dateA = new DateTime(1995, 10, 20, 8, 0, 0);
            var dateB = new DateTime(1995, 10, 20, 22, 30, 15);
            Assert.True(_comparer.AreEqual(dateA, dateB));
        }

        [Fact]
        public void AreEqual_ShouldReturnFalse_ForDifferentDates()
        {
            var dateA = new DateTime(1995, 10, 20);
            var dateB = new DateTime(2005, 1, 1);
            Assert.False(_comparer.AreEqual(dateA, dateB));
        }

        [Theory]
        [InlineData(true)] 
        [InlineData(false)]
        public void AreEqual_ShouldReturnFalse_WhenOneDateIsNull(bool aIsNull)
        {
            var date = new DateTime(1995, 10, 20);
            DateTime? a = aIsNull ? null : date;
            DateTime? b = aIsNull ? date : null;
            Assert.False(_comparer.AreEqual(a, b));
        }
    }
}