namespace BFB.Core.DTOs
{
	public class SignUpDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public UserIntroductionDto UserIntroduction { get; set; }
	}
}
