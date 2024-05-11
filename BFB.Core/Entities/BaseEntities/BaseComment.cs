namespace BFB.Core.Entities.BaseEntities
{
	public abstract class BaseComment : BaseEntityNoName
	{
		public AppUser User { get; set; }
		public string UserId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public bool IsDeleted { get; set; }
	}
}
