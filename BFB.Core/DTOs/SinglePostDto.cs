namespace BFB.Core.DTOs
{
	public class SinglePostDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public UserDto User { get; set; }
		public ICollection<LikesDto> Likes { get; set; }
		public int LikesCount { get; set; }
		public ICollection<CommentsDto> Comments { get; set; }
		public int CommentsCount { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
