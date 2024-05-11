namespace BFB.Core.DTOs
{
	public class CommentsDto
	{
		public Guid Id { get; set; }
		public UserDto User { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
