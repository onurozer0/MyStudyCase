using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class PrivateMessage : BaseEntityNoName
	{
		public Guid ConversationId { get; set; }
		public Conversation Conversation { get; set; }
		public string SenderId { get; set; }
		public AppUser Sender { get; set; }
		public string ReceiverId { get; set; }
		public AppUser Receiver { get; set; }
		public string Message { get; set; }
	}
}
