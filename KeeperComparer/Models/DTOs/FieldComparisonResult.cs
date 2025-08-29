namespace KeeperComparer.Models.DTOs
{
    public enum ComparisonStatus
    {
        NotCompared,
        ExactMatch,
        SimilarMatch,
        Mismatch,
        BothEmpty,
        OneIsEmpty
    }
    public class FieldComparisonResult
    {
        public string FieldName { get; set; }
        public string ValueA { get; set; }
        public string ValueB { get; set; }
        public ComparisonStatus Status { get; set; }
        public double? SimilarityScore { get; set; } = null;
    }
}