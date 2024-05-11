using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface ICategoryService : IService<Category>
	{
		Task<Response<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
		Task<Response<IEnumerable<CategoryDto>>> GetSubCategoriesAsync();
		Task<Response<IEnumerable<CategoryDto>>> GetParentCategoriesAsync();
		Task<Response<CategoryDto>> GetCategoryByIdAsync(Guid id);
		Task<Response<NoDataDto>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
		Task<Response<CategoryDto>> AddCategoryAsync(CategoryAddDto categoryAddDto);
		Task<Response<NoDataDto>> DeleteCategoryAsync(Guid id);
		Task<Response<CategoryWithProductsDto>> GetCategoryWithProductsAsync(Guid id);

	}
}
