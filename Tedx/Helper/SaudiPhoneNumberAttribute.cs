namespace Tedx.Helper
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class SaudiPhoneNumberAttribute : ValidationAttribute
    {
        public SaudiPhoneNumberAttribute()
        {
            ErrorMessage = "رقم الجوال غير صحيح.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Let the Required attribute handle empty values
            }

            string phoneNumber = value.ToString();

            if (BeAValidSaudiPhoneNumber(phoneNumber))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

        private bool BeAValidSaudiPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^(?:\+966|966)?5\d{8}$");
        }
    }
}
