using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Conversation : BaseEntityNoUpdateDate
	{
		public string Title { get; set; }
		public ICollection<PrivateMessage> Messages { get; set; }
		public AppUser Starter { get; set; }
		public string StarterId { get; set; }
		public AppUser Receiver { get; set; }
		public string ReceiverId { get; set; }

	}
}
