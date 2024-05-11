using System.ComponentModel.DataAnnotations.Schema;

namespace BFB.Core.Entities.BaseEntities
{
	public abstract class BaseEntityNoUpdateDate : BaseEntityNoName
	{
		[NotMapped]
		public DateTime UpdatedDate { get; set; }
	}
}
