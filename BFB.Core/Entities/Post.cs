using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Post : BaseEntityNoName
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public ICollection<PostLike> Likes { get; set; }
		public ICollection<PostComment> Comments { get; set; }
		public bool IsActive { get; set; }
	}
}
