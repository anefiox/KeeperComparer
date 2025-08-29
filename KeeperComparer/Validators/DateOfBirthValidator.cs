using KeeperComparer.Interfaces;
using System;

namespace KeeperComparer.Validators
{
    public class DateOfBirthValidator : IValidator<DateTime>
    {
        public bool IsValid(DateTime dob)
        {
            var today = DateTime.UtcNow.Date;
            if (dob.Date >= today)
                return false;

            if (dob == default(DateTime))
                return false;

            return true;
        }
    }
}