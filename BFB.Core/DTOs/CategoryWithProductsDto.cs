namespace BFB.Core.DTOs
{
	public class CategoryWithProductsDto
	{
		public ICollection<ProductDto> Products { get; set; }
		public CategoryDto Category { get; set; }
	}
}
