namespace BFB.Core.Entities
{
	public class Like
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
