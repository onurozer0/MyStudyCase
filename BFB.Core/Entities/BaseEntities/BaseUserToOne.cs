using System.ComponentModel.DataAnnotations;

namespace BFB.Core.Entities.BaseEntities
{
	public abstract class BaseUserToOne
	{
		[Key]
		public string UserId { get; set; }
		public AppUser User { get; set; }
	}
}
