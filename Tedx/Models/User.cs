
using Humanizer.Localisation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Tedx.Helper;

namespace Tedx.Models
{
	public class User()
	{
   
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "FullNameRequired")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "AgeRequired")]
        public int Age { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "RoleIsRequired")]
        public string RoleAs { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "JobRequired")]
        public string Job { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "EmailVaildation")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "PhoneRequired")]
        [SaudiPhoneNumber(ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "PhoneValidation")]
        public string Phone { get; set; }

        public string ?IdeaCategory { get; set; }


        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.User), ErrorMessageResourceName = "MaxWordsValidation")]
        public string? IdeaDescription { get; set; }
        public string ?WhyIdea { get; set; }
        public bool? HasPresentedBefore { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
	
	}
}
