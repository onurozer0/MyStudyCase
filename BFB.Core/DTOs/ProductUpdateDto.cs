namespace BFB.Core.DTOs
{
	public class ProductUpdateDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid CategoryId { get; set; }
	}
}
