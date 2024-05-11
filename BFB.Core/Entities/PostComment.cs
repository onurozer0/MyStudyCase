using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class PostComment : BaseComment
	{
		public Post Post { get; set; }
		public Guid PostId { get; set; }
	}
}
