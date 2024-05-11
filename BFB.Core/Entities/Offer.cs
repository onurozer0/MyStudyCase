using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Offer : BaseEntityNoName
	{
		public Product Product { get; set; }
		public Guid ProductId { get; set; }
		public AppUser Offerer { get; set; }
		public AppUser Receiver { get; set; }
		public string OffererId { get; set; }
		public string ReceiverId { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public decimal Price { get; set; }
		public bool IsAccepted { get; set; } = false;
		public bool IsHandled { get; set; } = false;
	}
}
