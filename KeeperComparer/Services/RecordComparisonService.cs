using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using KeeperComparer.Models.DTOs;

namespace KeeperComparer.Services
{
    public class RecordComparisonService
    {
        private readonly ITextComparer _nameComparer;
        private readonly IEmailComparer _emailComparer;
        private readonly IDateOfBirthComparer _dobComparer;
        private readonly INumberComparer _mobileComparer;
        private readonly INumberComparer _landlineComparer;
        private readonly IAddressComparer _addressComparer;
        private readonly IPostcodeComparer _postcodeComparer;

        public RecordComparisonService(
            ITextComparer nameComparer,
            IEmailComparer emailComparer,
            IDateOfBirthComparer dobComparer,
            INumberComparer mobileComparer,
            INumberComparer landlineComparer,
            IAddressComparer addressComparer,
            IPostcodeComparer postcodeComparer)
        {
            _nameComparer = nameComparer;
            _emailComparer = emailComparer;
            _dobComparer = dobComparer;
            _mobileComparer = mobileComparer;
            _landlineComparer = landlineComparer;
            _addressComparer = addressComparer;
            _postcodeComparer = postcodeComparer;
        }

        public ComparisonResult CompareRecords(KeeperRecord recordA, KeeperRecord recordB)
        {
            var result = new ComparisonResult();

            CompareTextField(result, "Full Name", recordA.FullName, recordB.FullName, _nameComparer);
            CompareDateField(result, "Date of Birth", recordA.DateOfBirth, recordB.DateOfBirth);
            CompareTextField(result, "Email Address", recordA.EmailAddress, recordB.EmailAddress, _emailComparer);
            CompareNumberField(result, "Mobile Number", recordA.MobileNumber, recordB.MobileNumber, _mobileComparer);
            CompareNumberField(result, "Landline Number", recordA.LandlineNumber, recordB.LandlineNumber, _landlineComparer);

            CompareAddressField(result, recordA.Address, recordB.Address);

            result.OverallResult = DetermineOverallMatch(result.FieldResults);
            return result;
        }

        private void CompareAddressField(ComparisonResult result, Address? valA, Address? valB)
        {
            CompareTextField(result, "Address Line 1", valA?.Line1, valB?.Line1, _nameComparer);
            CompareTextField(result, "City", valA?.City, valB?.City, _nameComparer);
            CompareTextField(result, "Postcode", valA?.Postcode, valB?.Postcode, _postcodeComparer);
        }

        private void CompareTextField(ComparisonResult result, string fieldName, string? valA, string? valB, ITextComparer comparer)
        {
            var fieldResult = new FieldComparisonResult { FieldName = fieldName, ValueA = valA ?? "", ValueB = valB ?? "" };
            if (string.IsNullOrWhiteSpace(valA) && string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.BothEmpty;
            else if (string.IsNullOrWhiteSpace(valA) || string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.OneIsEmpty;
            else if (comparer.AreEqual(valA, valB)) fieldResult.Status = ComparisonStatus.ExactMatch;
            else if (comparer.AreSimilar(valA, valB))
            {
                fieldResult.Status = ComparisonStatus.SimilarMatch;
                fieldResult.SimilarityScore = comparer.SimilarityScore(valA, valB);
            }
            else fieldResult.Status = ComparisonStatus.Mismatch;
            result.FieldResults.Add(fieldResult);
        }

        private void CompareTextField(ComparisonResult result, string fieldName, string? valA, string? valB, IEqualityComparer comparer)
        {
            var fieldResult = new FieldComparisonResult { FieldName = fieldName, ValueA = valA ?? "", ValueB = valB ?? "" };
            if (string.IsNullOrWhiteSpace(valA) && string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.BothEmpty;
            else if (string.IsNullOrWhiteSpace(valA) || string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.OneIsEmpty;
            else if (comparer.AreEqual(valA, valB)) fieldResult.Status = ComparisonStatus.ExactMatch;
            else fieldResult.Status = ComparisonStatus.Mismatch;
            result.FieldResults.Add(fieldResult);
        }

        private void CompareDateField(ComparisonResult result, string fieldName, DateTime? valA, DateTime? valB)
        {
            var fieldResult = new FieldComparisonResult { FieldName = fieldName, ValueA = valA?.ToShortDateString() ?? "", ValueB = valB?.ToShortDateString() ?? "" };
            if (!valA.HasValue && !valB.HasValue) fieldResult.Status = ComparisonStatus.BothEmpty;
            else if (!valA.HasValue || !valB.HasValue) fieldResult.Status = ComparisonStatus.OneIsEmpty;
            else if (_dobComparer.AreEqual(valA, valB)) fieldResult.Status = ComparisonStatus.ExactMatch;
            else fieldResult.Status = ComparisonStatus.Mismatch;
            result.FieldResults.Add(fieldResult);
        }

        private void CompareNumberField(ComparisonResult result, string fieldName, string? valA, string? valB, INumberComparer comparer)
        {
            var fieldResult = new FieldComparisonResult { FieldName = fieldName, ValueA = valA ?? "", ValueB = valB ?? "" };
            if (string.IsNullOrWhiteSpace(valA) && string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.BothEmpty;
            else if (string.IsNullOrWhiteSpace(valA) || string.IsNullOrWhiteSpace(valB)) fieldResult.Status = ComparisonStatus.OneIsEmpty;
            else if (comparer.AreEqual(valA, valB)) fieldResult.Status = ComparisonStatus.ExactMatch;
            else fieldResult.Status = ComparisonStatus.Mismatch;
            result.FieldResults.Add(fieldResult);
        }

        private MatchStrength DetermineOverallMatch(List<FieldComparisonResult> fieldResults)
        {
            if (fieldResults.All(fr => fr.Status == ComparisonStatus.ExactMatch || fr.Status == ComparisonStatus.BothEmpty))
            {
                return MatchStrength.Exact;
            }

            var scores = fieldResults.ToDictionary(fr => fr.FieldName, fr => fr.Status);

            var isEmailMatch = scores.GetValueOrDefault("EmailAddress") == ComparisonStatus.ExactMatch;
            var isDobMatch = scores.GetValueOrDefault("DateofBirth") == ComparisonStatus.ExactMatch;
            var isNameSimilar = scores.GetValueOrDefault("FullName") == ComparisonStatus.ExactMatch || scores.GetValueOrDefault("FullName") == ComparisonStatus.SimilarMatch;

            if (isEmailMatch && (isDobMatch || isNameSimilar))
            {
                return MatchStrength.Strong;
            }

            if (isDobMatch && isNameSimilar)
            {
                return MatchStrength.Strong;
            }

            var isPostcodeMatch = scores.GetValueOrDefault("Postcode") == ComparisonStatus.ExactMatch;
            if (isNameSimilar && isPostcodeMatch)
            {
                return MatchStrength.Partial;
            }

            if (isNameSimilar)
            {
                return MatchStrength.Weak;
            }

            return MatchStrength.NoMatch;
        }
    }
}