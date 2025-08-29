using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Models.DTOs;
using KeeperComparer.Services;
using Moq;
using System;
using Xunit;

namespace KeeperComparer.Tests.Services
{
    public class RecordComparisonServiceTests
    {
        private readonly Mock<ITextComparer> _mockNameComparer;
        private readonly Mock<IEmailComparer> _mockEmailComparer;
        private readonly Mock<IDateOfBirthComparer> _mockDobComparer;
        private readonly Mock<INumberComparer> _mockMobileComparer;
        private readonly Mock<INumberComparer> _mockLandlineComparer;
        private readonly Mock<IAddressComparer> _mockAddressComparer;
        private readonly Mock<IPostcodeComparer> _mockPostcodeComparer;

        private readonly RecordComparisonService _service;

        private readonly KeeperRecord _recordA;
        private readonly KeeperRecord _recordB;

        public RecordComparisonServiceTests()
        {
            _mockNameComparer = new Mock<ITextComparer>();
            _mockEmailComparer = new Mock<IEmailComparer>();
            _mockDobComparer = new Mock<IDateOfBirthComparer>();
            _mockMobileComparer = new Mock<INumberComparer>();
            _mockLandlineComparer = new Mock<INumberComparer>();
            _mockAddressComparer = new Mock<IAddressComparer>();
            _mockPostcodeComparer = new Mock<IPostcodeComparer>();

            _service = new RecordComparisonService(
                _mockNameComparer.Object,
                _mockEmailComparer.Object,
                _mockDobComparer.Object,
                _mockMobileComparer.Object,
                _mockLandlineComparer.Object,
                _mockAddressComparer.Object,
                _mockPostcodeComparer.Object
            );

            _recordA = new KeeperRecord { FullName = "Jon Smith", EmailAddress = "a@b.com", DateOfBirth = new DateTime(1990, 1, 1), Address = new Address { Postcode = "SW1A 0AA" } };
            _recordB = new KeeperRecord { FullName = "John Smith", EmailAddress = "x@y.com", DateOfBirth = new DateTime(1990, 1, 1), Address = new Address { Postcode = "SW1A 0AA" } };
        }

        private void SetupAllMocksToMismatch()
        {
            _mockNameComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockNameComparer.Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(false);
            _mockEmailComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            _mockDobComparer.Setup(c => c.AreEqual(It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(false);
            _mockPostcodeComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        }

        [Fact]
        public void CompareRecords_WithExactEmailMatch_ReturnsStrongMatch()
        {
            SetupAllMocksToMismatch();
            _mockEmailComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _service.CompareRecords(_recordA, _recordB);

            Assert.Equal(MatchStrength.Strong, result.OverallResult);
        }

        [Fact]
        public void CompareRecords_WithExactDobAndSimilarName_ReturnsStrongMatch()
        {
            SetupAllMocksToMismatch();
            _mockDobComparer.Setup(c => c.AreEqual(It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(true);
            _mockNameComparer.Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(true);

            var result = _service.CompareRecords(_recordA, _recordB);

            Assert.Equal(MatchStrength.Strong, result.OverallResult);
        }

        [Fact]
        public void CompareRecords_WithSimilarNameAndMatchingPostcode_ReturnsPartialMatch()
        {
            SetupAllMocksToMismatch();
            _mockNameComparer.Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(true);
            _mockPostcodeComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _service.CompareRecords(_recordA, _recordB);

            Assert.Equal(MatchStrength.Partial, result.OverallResult);
        }

        [Fact]
        public void CompareRecords_WithOnlySimilarName_ReturnsWeakMatch()
        {
            SetupAllMocksToMismatch();
            _mockNameComparer.Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(true);

            var result = _service.CompareRecords(_recordA, _recordB);

            Assert.Equal(MatchStrength.Weak, result.OverallResult);
        }

        [Fact]
        public void CompareRecords_WithNoMatchingFields_ReturnsNoMatch()
        {
            SetupAllMocksToMismatch();

            var result = _service.CompareRecords(_recordA, _recordB);

            Assert.Equal(MatchStrength.NoMatch, result.OverallResult);
        }

        [Fact]
        public void CompareRecords_CorrectlyPopulatesFieldResults()
        {
            SetupAllMocksToMismatch();
            _mockEmailComparer.Setup(c => c.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockNameComparer.Setup(c => c.AreSimilar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(true); 

            var result = _service.CompareRecords(_recordA, _recordB);

            var emailResult = result.FieldResults.Find(fr => fr.FieldName == "Email Address");
            var nameResult = result.FieldResults.Find(fr => fr.FieldName == "Full Name");
            var dobResult = result.FieldResults.Find(fr => fr.FieldName == "Date of Birth");

            Assert.Equal(ComparisonStatus.ExactMatch, emailResult.Status);
            Assert.Equal(ComparisonStatus.SimilarMatch, nameResult.Status);
            Assert.Equal(ComparisonStatus.Mismatch, dobResult.Status);
        }
    }
}