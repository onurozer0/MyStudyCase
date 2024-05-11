namespace BFB.Core.DTOs
{
	public class UserProfileDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Surname { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public int LikesCount { get; set; }
		public int CommentsCount { get; set; }
		public int MessagesSent { get; set; }
		public int OffersApproved { get; set; }
		public int OffersSent { get; set; }
		public int ProfileVisitsCount { get; set; }
		public IEnumerable<ProductDto> Products { get; set; }
	}
}
