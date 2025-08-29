using KeeperComparer.Interfaces;

namespace KeeperComparer.Services
{
    public class DateOfBirthComparer : IDateOfBirthComparer
    {
        public bool AreEqual(DateTime? a, DateTime? b)
        {
            if (!a.HasValue || !b.HasValue)
                return false;

            return a.Value.Date == b.Value.Date;
        }
    }
}