namespace KeeperComparer.Interfaces
{
    public interface ITextComparer
    {
        bool AreEqual(string a, string b);
        bool AreSimilar(string a, string b, double threshold = 0.85);
        double SimilarityScore(string a, string b);
    }
}