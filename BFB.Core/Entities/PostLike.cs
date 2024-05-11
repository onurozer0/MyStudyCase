namespace BFB.Core.Entities
{
	public class PostLike
	{
		public Guid Id { get; set; }
		public Guid PostId { get; set; }
		public Post Post { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
