using BFB.AdminAPI.Filters;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class CategoryController : CustomControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> Sub()
		{
			return ActionResultInstance(await _categoryService.GetSubCategoriesAsync());
		}
		[HttpGet]
		public async Task<IActionResult> Parent()
		{
			return ActionResultInstance(await _categoryService.GetParentCategoriesAsync());
		}
		[HttpPut]
		public async Task<IActionResult> Update(CategoryUpdateDto dto)
		{
			return ActionResultInstance(await _categoryService.UpdateCategoryAsync(dto));
		}
		[HttpPost]
		public async Task<IActionResult> Add(CategoryAddDto dto)
		{
			return ActionResultInstance(await _categoryService.AddCategoryAsync(dto));
		}
		[HttpDelete]
		[ServiceFilter(typeof(NotFoundFilter<Category>))]
		[Route("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _categoryService.DeleteCategoryAsync(id));
		}

	}
}
