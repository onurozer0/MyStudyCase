using BFB.AdminAPI.Filters;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class CommentController : CustomControllerBase
	{
		private readonly ICommentService _commentService;
		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}
		[HttpGet]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Post>))]
		public async Task<IActionResult> Post(Guid id)
		{
			return ActionResultInstance(await _commentService.GetPostCommentsByIdAsync(id));
		}
		[HttpGet]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		public async Task<IActionResult> Product(Guid id)
		{
			return ActionResultInstance(await _commentService.GetCommentsByProductIdAsync(id));
		}
		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> Users(string id)
		{
			return ActionResultInstance(await _commentService.GetCommentsByUserAsync(id));
		}
		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _commentService.DeleteCommentAsync(id));
		}
		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeletePost(Guid id)
		{
			return ActionResultInstance(await _commentService.DeletePostCommentAsync(id));
		}

	}
}
