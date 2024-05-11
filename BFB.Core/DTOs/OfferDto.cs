namespace BFB.Core.DTOs
{
	public class OfferDto
	{
		public Guid Id { get; set; }
		public ProductDto Product { get; set; }
		public UserDto Offerer { get; set; }
		public UserDto Receiver { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public decimal Price { get; set; }
		public bool IsAccepted { get; set; }
		public bool IsHandled { get; set; }
		public DateTime CreatedDate { get; set; }

	}
}
