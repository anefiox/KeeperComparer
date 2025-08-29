using KeeperComparer.Interfaces;
using KeeperComparer.Models;

namespace KeeperComparer.Validators
{
    public class AddressValidator : IValidator<Address>
    {
        private readonly IPostcodeValidator _postcodeValidator;
        public AddressValidator(IPostcodeValidator postcodeValidator)
        {
            _postcodeValidator = postcodeValidator;
        }

        public bool IsValid(Address? address)
        {
            if (address == null)
                return false;

            if (string.IsNullOrWhiteSpace(address.Line1))
                return false;

            if (!_postcodeValidator.IsValid(address.Postcode))
                return false;

            return true;
        }
    }
}