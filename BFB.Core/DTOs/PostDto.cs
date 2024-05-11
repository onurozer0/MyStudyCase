namespace BFB.Core.DTOs
{
	public class PostDto
	{
		public Guid Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime? UpdatedDate { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string UserId { get; set; }
		public UserDto User { get; set; }
		public int LikesCount { get; set; }
		public int CommentsCount { get; set; }
	}
}
