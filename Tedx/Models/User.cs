namespace Tedx.Models
{
	public class User
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public int Age { get; set; }
		public RoleAs Role { get; set; } // Speaker or Listener
		public string Job { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
        public string ?IdeaCategory { get; set; }
        public string ?IdeaDescription { get; set; }
        public string ?WhyIdea { get; set; }
        public bool? HasPresentedBefore { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
	
	}
}
