using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public ICollection<Product>? Products { get; set; }
		public Guid? ParentID { get; set; }
		public Category? ParentCategory { get; set; }
		public ICollection<Category>? SubCategories { get; set; }
	}
}
