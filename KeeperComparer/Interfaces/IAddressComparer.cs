using KeeperComparer.Models;

namespace KeeperComparer.Interfaces
{
    public interface IAddressComparer
    {
        bool AreEqual(Address? a, Address? b);
        bool AreSimilar(Address? a, Address? b);
    }
}