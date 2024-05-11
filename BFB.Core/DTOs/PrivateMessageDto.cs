namespace BFB.Core.DTOs
{
	public class PrivateMessageDto
	{
		public DateTime CreatedDate { get; set; }
		public Guid Id { get; set; }
		public string Message { get; set; }
		public UserDto Sender { get; set; }
		public UserDto Receiver { get; set; }
	}
}
