using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Comment : BaseComment
	{
		public Product Product { get; set; }
		public Guid ProductId { get; set; }
	}
}
