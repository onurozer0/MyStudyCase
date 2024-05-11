using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class UserRefreshToken : BaseUserToOne
	{
		public string Code { get; set; }
		public DateTime ExpireDate { get; set; }
	}
}
