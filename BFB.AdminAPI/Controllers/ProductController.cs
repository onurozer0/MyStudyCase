using BFB.AdminAPI.Filters;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class ProductController : CustomControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		[HttpGet]
		public async Task<IActionResult> NonActive()
		{
			return ActionResultInstance(await _productService.GetNotActiveProductsAsync());
		}
		[HttpGet]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		[Route("{id}")]
		public async Task<IActionResult> Activate(Guid id)
		{
			return ActionResultInstance(await _productService.ActivateProductAsync(id));
		}
		[HttpGet]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		[Route("{id}")]
		public async Task<IActionResult> Deactivate(Guid id)
		{
			return ActionResultInstance(await _productService.DeactivateProductAsync(id));
		}
		[HttpGet]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		[Route("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _productService.DeleteProductAsync(id));
		}
		[HttpGet]
		public async Task<IActionResult> MostComment()
		{
			return ActionResultInstance(await _productService.GetMostCommentProductsAsync());
		}
		[HttpGet]
		public async Task<IActionResult> MostLiked()
		{
			return ActionResultInstance(await _productService.GetMostLikedProductsAsync());
		}
	}
}
