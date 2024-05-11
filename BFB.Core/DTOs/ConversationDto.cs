namespace BFB.Core.DTOs
{
	public class ConversationDto
	{
		public Guid Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Title { get; set; }
		public UserDto Starter { get; set; }
		public UserDto Receiver { get; set; }
		public ICollection<PrivateMessageDto> Messages { get; set; }
	}
}
