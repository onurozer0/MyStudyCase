using BFB.Core.Models;

namespace BFB.Core.DTOs
{
	public class UserAddressesDto
	{
		public City? City { get; set; }
		public string? Address { get; set; }
		public string? Zipcode { get; set; }
	}
}
