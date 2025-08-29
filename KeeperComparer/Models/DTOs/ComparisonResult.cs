using System.Collections.Generic;

namespace KeeperComparer.Models.DTOs
{
    public enum MatchStrength
    {
        NoMatch,
        Weak,
        Partial,
        Strong,
        Exact
    }

    public class ComparisonResult
    {
        public MatchStrength OverallResult { get; set; }
        public List<FieldComparisonResult> FieldResults { get; set; } = new List<FieldComparisonResult>();
    }
}