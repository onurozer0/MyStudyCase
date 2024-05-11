namespace BFB.Core.DTOs
{
	public class UpdateMemberDto
	{
		public string Password { get; set; }
		public string PasswordConfirm { get; set; }
		public UserAddressesDto? UserAddress { get; set; }
		public string? Description { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
	}
}
