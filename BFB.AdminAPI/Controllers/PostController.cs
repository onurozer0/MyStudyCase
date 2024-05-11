using BFB.AdminAPI.Filters;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class PostController : CustomControllerBase
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}
		[HttpDelete]
		[ServiceFilter(typeof(NotFoundFilter<Post>))]
		[Route("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _postService.DeletePostAsync(id));
		}
		[HttpGet]
		public async Task<IActionResult> MostLiked()
		{
			return ActionResultInstance(await _postService.GetMostLikedPostsAsync());
		}
		[HttpGet]
		public async Task<IActionResult> MostComment()
		{
			return ActionResultInstance(await _postService.GetMostCommentPostsAsync());
		}
		[HttpGet]
		public async Task<IActionResult> NonActive()
		{
			return ActionResultInstance(await _postService.GetActiveOrNotActivePostsAsync(false));
		}
	}
}
