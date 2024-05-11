namespace BFB.Core.Entities.BaseEntities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime? UpdatedDate { get; set; }
	}
}
