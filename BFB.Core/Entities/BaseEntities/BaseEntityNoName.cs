using System.ComponentModel.DataAnnotations.Schema;

namespace BFB.Core.Entities.BaseEntities
{
	public abstract class BaseEntityNoName : BaseEntity
	{
		[NotMapped]
		public string Name { get; set; }
	}
}
