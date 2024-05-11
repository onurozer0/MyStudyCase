namespace BFB.Core.DTOs
{
	public class SendPrivateMessageDto
	{
		public Guid ConversationId { get; set; }
		public string Message { get; set; }
	}
}
