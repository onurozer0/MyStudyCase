namespace BFB.Core.DTOs
{
	public class UnregisteredUserDto
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool IsConfirmed { get; set; }
		public DateTime CreatedDate { get; set; }
		public UserIntroductionDto UserIntroduction { get; set; }
	}
}
