using BFB.AdminAPI.Controllers;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	public class ProductController : CustomControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return ActionResultInstance(await _productService.GetActiveProductsAsync());
		}
		[HttpDelete]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		public async Task<IActionResult> Remove(Guid id)
		{
			return ActionResultInstance(await _productService.RemoveProductAsync(id));
		}
		[HttpPut]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Update(ProductUpdateDto dto)
		{
			return ActionResultInstance(await _productService.UpdateProductAsync(dto));
		}
		[HttpGet]
		[Route("/api/product/{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		public async Task<IActionResult> Get(Guid id)
		{
			return ActionResultInstance(await _productService.GetProductDetailsAsync(id));
		}
		[HttpPost]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Add(ProductAddDto dto)
		{
			return ActionResultInstance(await _productService.AddProductAsync(dto));
		}
		[HttpPost]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> AddComment(CommentAddDto dto)
		{
			return ActionResultInstance(await _productService.AddCommentAsync(dto));
		}
		[HttpGet]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		[Route("/api/product/like/{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		public async Task<IActionResult> AddLike(Guid id)
		{
			return ActionResultInstance(await _productService.AddLikeAsync(id));
		}
		[HttpGet]
		[Route("/api/product/user/{userId}")]
		public async Task<IActionResult> Users(string userId)
		{
			return ActionResultInstance(await _productService.GetProductsByUserIdAsync(userId));
		}

	}
}
