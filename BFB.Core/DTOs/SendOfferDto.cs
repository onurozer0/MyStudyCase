namespace BFB.Core.DTOs
{
	public class SendOfferDto
	{
		public Guid ProductId { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public decimal Price { get; set; }
	}
}
