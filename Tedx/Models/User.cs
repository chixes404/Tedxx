using System.ComponentModel.DataAnnotations;

namespace Tedx.Models
{
	public class User
	{
		public int Id { get; set; }

        [Required(ErrorMessage = "الاسم كامل مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "العمر مطلوب")]
        public int Age { get; set; }

        [Required(ErrorMessage = "الانضمام كـ مطلوب")]
        public string RoleAs { get; set; }

        [Required(ErrorMessage = "الوظيفة مطلوبة")]
        public string Job { get; set; }

        [Required(ErrorMessage = "الايميل مطلوب")]
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الجوال مطلوب")]
        public string Phone { get; set; }

        public string ?IdeaCategory { get; set; }

        [MaxLength(300)]
        public string ?IdeaDescription { get; set; }
        public string ?WhyIdea { get; set; }
        public bool? HasPresentedBefore { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
	
	}
}
