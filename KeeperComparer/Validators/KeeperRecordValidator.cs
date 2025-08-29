using KeeperComparer.Interfaces;
using KeeperComparer.Models;
using System;
using System.Collections.Generic;

namespace KeeperComparer.Validators
{
    public class KeeperRecordValidator
    {
        private readonly IEmailValidator _emailValidator;
        private readonly IMobileNumberValidator _mobileValidator;
        private readonly ILandlineNumberValidator _landlineValidator;
        private readonly IValidator<DateTime> _dobValidator;
        private readonly IPostcodeValidator _postcodeValidator;

        public KeeperRecordValidator(
            IEmailValidator emailValidator,
            IMobileNumberValidator mobileValidator,
            ILandlineNumberValidator landlineValidator,
            IValidator<DateTime> dobValidator,
            IPostcodeValidator postcodeValidator)
        {
            _emailValidator = emailValidator;
            _mobileValidator = mobileValidator;
            _landlineValidator = landlineValidator;
            _dobValidator = dobValidator;
            _postcodeValidator = postcodeValidator;
        }

        public (bool IsValid, List<string> Errors) Validate(KeeperRecord record)
        {
            var errors = new List<string>();

            if (record == null)
            {
                errors.Add("Record cannot be null.");
                return (false, errors);
            }

            if (string.IsNullOrWhiteSpace(record.FullName))
                errors.Add("Full Name is required.");

            if (record.DateOfBirth.HasValue && !_dobValidator.IsValid(record.DateOfBirth.Value))
                errors.Add("Date of Birth is invalid.");

            if (!string.IsNullOrWhiteSpace(record.EmailAddress) && !_emailValidator.IsValid(record.EmailAddress))
                errors.Add("Email Address has an invalid format.");

            if (!string.IsNullOrWhiteSpace(record.MobileNumber) && !_mobileValidator.IsValid(record.MobileNumber))
                errors.Add("Mobile Number has an invalid format.");

            if (!string.IsNullOrWhiteSpace(record.LandlineNumber) && !_landlineValidator.IsValid(record.LandlineNumber))
                errors.Add("Landline Number has an invalid format.");

            if (record.Address != null && !_postcodeValidator.IsValid(record.Address.Postcode))
                errors.Add("Postcode has an invalid format.");

            return (errors.Count == 0, errors);
        }
    }
}