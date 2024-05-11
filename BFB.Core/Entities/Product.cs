using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Category Category { get; set; }
		public Guid CategoryId { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public ICollection<Like> Likes { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Offer> Offers { get; set; }
		public bool IsActive { get; set; }
	}
}
