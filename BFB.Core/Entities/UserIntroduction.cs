using BFB.Core.Entities.BaseEntities;

namespace BFB.Core.Entities
{
	public class UserIntroduction : BaseUserToOne
	{
		public string Message { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ApprovedDate { get; set; }
	}
}
