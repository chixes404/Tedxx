
using Humanizer.Localisation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Tedx.Helper;

namespace Tedx.Models
{
	public class User()
	{
   
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "FullNameRequired")]
        [Length(2, 30, ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "FullNameLength")]
        [RegularExpression(@"^[^\d]*$", ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "FullNameLettersOnly")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "AgeRequired")]
        public string Age { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "RoleIsRequired")]
        public string RoleAs { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "JobRequired")]
        public string Job { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "EmailVaildation")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "PhoneRequired")]
        [SaudiPhoneNumber(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "PhoneInvalid")]
        public string Phone { get; set; }

        public string? QRCodeUrl { get; set; }



        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "MaxWordsValidation")]

        public string ?ListenAboutEvent { get; set; }
        public bool? HasChildIn { get; set; }


        public string ?IdeaCategory { get; set; }


        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "MaxWordsValidation")]
        public string? IdeaDescription { get; set; }
        public string ?WhyIdea { get; set; }
        public bool? HasPresentedBefore { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
	
	}
    public class SaudiPhoneNumberAttribute : ValidationAttribute
    {
      
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
            return Regex.IsMatch(phoneNumber, @"^(\+966|966|0)?5\d{8}$");
        }
    }


    //public class AgeValidationAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
    //        {
    //            return ValidationResult.Success; // Allow null or empty values (use [Required] for mandatory fields)
    //        }

    //        // Convert Arabic numerals to Western numerals
    //        string ageString = value.ToString();
    //        string westernAgeString = ConvertArabicNumeralsToWestern(ageString);

    //        // Try to parse the age as an integer
    //        if (int.TryParse(westernAgeString, out int age))
    //        {
    //            if (age >= 0 && age <= 150) // Example: Validate age range
    //            {
    //                return ValidationResult.Success;
    //            }
    //            else
    //            {
    //                return new ValidationResult(GetErrorMessage(validationContext, "AgeValidation"));
    //            }
    //        }
    //        else
    //        {
    //            return new ValidationResult(GetErrorMessage(validationContext, "AgeValidation"));
    //        }
    //    }

    //    private string GetErrorMessage(ValidationContext validationContext, string errorMessageKey)
    //    {
    //        var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources.User>)) as IStringLocalizer<Resources.User>;
    //        return localizer?[errorMessageKey] ?? "Invalid age format or range.";
    //    }

    //    private string ConvertArabicNumeralsToWestern(string input)
    //    {
    //        // Map Arabic numerals to Western numerals
    //        var arabicToWesternMap = new Dictionary<char, char>
    //    {
    //        {'٠', '0'}, {'۰', '0'}, // Arabic-Indic and Persian zero
    //        {'١', '1'}, {'۱', '1'}, // Arabic-Indic and Persian one
    //        {'٢', '2'}, {'۲', '2'}, // Arabic-Indic and Persian two
    //        {'٣', '3'}, {'۳', '3'}, // Arabic-Indic and Persian three
    //        {'٤', '4'}, {'۴', '4'}, // Arabic-Indic and Persian four
    //        {'٥', '5'}, {'۵', '5'}, // Arabic-Indic and Persian five
    //        {'٦', '6'}, {'۶', '6'}, // Arabic-Indic and Persian six
    //        {'٧', '7'}, {'۷', '7'}, // Arabic-Indic and Persian seven
    //        {'٨', '8'}, {'۸', '8'}, // Arabic-Indic and Persian eight
    //        {'٩', '9'}, {'۹', '9'}  // Arabic-Indic and Persian nine
    //    };

    //        // Convert each character in the input string
    //        var result = new System.Text.StringBuilder();
    //        foreach (char c in input)
    //        {
    //            if (arabicToWesternMap.ContainsKey(c))
    //            {
    //                result.Append(arabicToWesternMap[c]);
    //            }
    //            else
    //            {
    //                result.Append(c);
    //            }
    //        }

    //        return result.ToString();
    //    }
    //}
}
