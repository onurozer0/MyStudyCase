using BFB.Core.Entities.BaseEntities;
using BFB.Core.Models;

namespace BFB.Core.Entities
{
	public class UserAddresses : BaseUserToOne
	{
		public City? City { get; set; }
		public string? Address { get; set; }
		public string? Zipcode { get; set; }

	}
}
