using Microsoft.AspNetCore.Identity;

namespace BFB.Core.Entities
{
	public class AppUser : IdentityUser
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Description { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public string? LastLoginIp { get; set; }
		public int ProfileVisitsCount { get; set; }
		public bool IsActive { get; set; }
		public bool IsCreatedProfile { get; set; }
		public bool IsConfirmed { get; set; }
		public ICollection<Product>? Products { get; set; }
		public ICollection<Like>? Likes { get; set; }
		public ICollection<PostLike>? PostLikes { get; set; }
		public ICollection<Comment>? Comments { get; set; }
		public ICollection<PostComment>? PostComments { get; set; }
		public ICollection<Post>? Posts { get; set; }
		public ICollection<PrivateMessage>? SendedMessages { get; set; }
		public ICollection<PrivateMessage>? ReceivedMessages { get; set; }
		public ICollection<Conversation> StartedConversations { get; set; }
		public ICollection<Conversation> ReceivedConversations { get; set; }
		public ICollection<Offer> RequestedOffers { get; set; }
		public ICollection<Offer> ReceivedOffers { get; set; }
		public UserAddresses? UserAddress { get; set; }
		public UserIntroduction? UserIntroduction { get; set; }
		public UserRefreshToken? UserRefreshToken { get; set; }
	}
}
