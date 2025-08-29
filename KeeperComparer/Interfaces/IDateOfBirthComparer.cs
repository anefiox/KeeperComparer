using System;

namespace KeeperComparer.Interfaces
{
    public interface IDateOfBirthComparer
    {
        bool AreEqual(DateTime? a, DateTime? b);
    }
}