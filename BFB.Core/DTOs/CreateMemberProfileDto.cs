namespace BFB.Core.DTOs
{
	public class CreateMemberProfileDto
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
		public string IdentityNumber { get; set; }
		public string? Description { get; set; }
		public DateTime DateOfBirth { get; set; }
		public UserAddressesDto? UserAddress { get; set; }
	}
}
