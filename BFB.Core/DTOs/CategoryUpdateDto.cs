namespace BFB.Core.DTOs
{
	public class CategoryUpdateDto
	{
		public Guid Id { get; set; }
		public Guid ParentId { get; set; }
		public string Name { get; set; }
	}
}
