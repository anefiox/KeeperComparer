namespace KeeperComparer.Interfaces
{
    public interface IEqualityComparer
    {
        bool AreEqual(string? a, string? b);
    }
}