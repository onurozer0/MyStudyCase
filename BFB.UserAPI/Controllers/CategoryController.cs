using BFB.AdminAPI.Controllers;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	[ServiceFilter(typeof(NotFoundFilter<Category>))]
	public class CategoryController : CustomControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return ActionResultInstance(await _categoryService.GetAllCategoriesAsync());
		}
		[HttpGet]
		[Route("/api/category/{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return ActionResultInstance(await _categoryService.GetCategoryByIdAsync(id));
		}
		[HttpGet]
		[Route("/api/category/withproducts/{id}")]
		public async Task<IActionResult> GetWithProducts(Guid id)
		{
			return ActionResultInstance(await _categoryService.GetCategoryWithProductsAsync(id));
		}
	}
}
