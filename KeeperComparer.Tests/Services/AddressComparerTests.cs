using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Services;

namespace KeeperComparer.Tests.Services
{
    public class MockTextComparer : ITextComparer
    {
        public bool AreEqualResult { get; set; } = true;
        public bool AreSimilarResult { get; set; } = true;
        public double SimilarityScoreResult { get; set; } = 1.0;

        public bool AreEqual(string a, string b) => AreEqualResult;
        public bool AreSimilar(string a, string b, double threshold = 0.85) => AreSimilarResult;
        public double SimilarityScore(string a, string b) => SimilarityScoreResult;
    }

    public class AddressComparerTests
    {
        private readonly MockTextComparer _mockTextComparer;
        private readonly AddressComparer _addressComparer;

        public AddressComparerTests()
        {
            _mockTextComparer = new MockTextComparer();
            _addressComparer = new AddressComparer(_mockTextComparer);
        }

        [Fact]
        public void AreEqual_ShouldReturnTrue_WhenAllFieldsAreEqual()
        {
            var addressA = new Address { Line1 = "1 Main St", Line2 = "Apt 2", City = "Anytown", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "1 Main St", Line2 = "Apt 2", City = "Anytown", Postcode = "sw1a 0aa" };
            _mockTextComparer.AreEqualResult = true;

            var result = _addressComparer.AreEqual(addressA, addressB);

            Assert.True(result);
        }

        [Fact]
        public void AreEqual_ShouldReturnFalse_WhenPostcodesDiffer()
        {
            var addressA = new Address { Line1 = "1 Main St", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "1 Main St", Postcode = "SW1A 0AB" };
            _mockTextComparer.AreEqualResult = true; 

            var result = _addressComparer.AreEqual(addressA, addressB);

            Assert.False(result);
        }

        [Fact]
        public void AreEqual_ShouldReturnFalse_WhenAnyTextFieldIsDifferent()
        {
            var addressA = new Address { Line1 = "1 Main St", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "2 Main St", Postcode = "SW1A 0AA" };
            _mockTextComparer.AreEqualResult = false;

            var result = _addressComparer.AreEqual(addressA, addressB);

            Assert.False(result);
        }

        [Fact]
        public void AreSimilar_ShouldReturnTrue_WhenTextFieldsAreSimilarAndPostcodesMatch()
        {
            var addressA = new Address { Line1 = "1 Main Street", City = "Anytown", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "1 Main St", City = "Anytown", Postcode = "sw1a 0aa" };
            _mockTextComparer.AreSimilarResult = true;

            var result = _addressComparer.AreSimilar(addressA, addressB);

            Assert.True(result);
        }

        [Fact]
        public void AreSimilar_ShouldReturnFalse_WhenPostcodesDoNotMatch()
        {
            var addressA = new Address { Line1 = "1 Main St", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "1 Main St", Postcode = "DIFFERENT" };
            _mockTextComparer.AreSimilarResult = true; 

            var result = _addressComparer.AreSimilar(addressA, addressB);

            Assert.False(result);
        }

        [Fact]
        public void AreSimilar_ShouldReturnFalse_WhenTextFieldsAreNotSimilar()
        {
            var addressA = new Address { Line1 = "1 Main St", Postcode = "SW1A 0AA" };
            var addressB = new Address { Line1 = "Totally Different Road", Postcode = "SW1A 0AA" };
            _mockTextComparer.AreSimilarResult = false;

            var result = _addressComparer.AreSimilar(addressA, addressB);

            Assert.False(result);
        }

        [Fact]
        public void AllMethods_ShouldReturnFalse_WhenBothAddressesAreNull()
        {
            Assert.False(_addressComparer.AreEqual(null, null));
            Assert.False(_addressComparer.AreSimilar(null, null));
        }

        [Fact]
        public void AllMethods_ShouldReturnFalse_WhenOneAddressIsNull()
        {
            var validAddress = new Address { Line1 = "123 Main St", Postcode = "TEST 123" };


            Assert.False(_addressComparer.AreEqual(validAddress, null));
            Assert.False(_addressComparer.AreEqual(null, validAddress));

            Assert.False(_addressComparer.AreSimilar(validAddress, null));
            Assert.False(_addressComparer.AreSimilar(null, validAddress));
        }
    }
}