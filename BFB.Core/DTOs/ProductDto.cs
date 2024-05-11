namespace BFB.Core.DTOs
{
	public class ProductDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Description { get; set; }
		public CategoryDto Category { get; set; }
		public UserDto User { get; set; }
		public int LikesCount { get; set; }
		public int CommentsCount { get; set; }
	}
}
