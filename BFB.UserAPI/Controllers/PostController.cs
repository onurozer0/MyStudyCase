using BFB.AdminAPI.Controllers;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	[ServiceFilter(typeof(NotFoundFilter<Post>))]
	public class PostController : CustomControllerBase
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}
		[HttpDelete]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		[Route("{id}")]
		public async Task<IActionResult> Remove(Guid id)
		{
			return ActionResultInstance(await _postService.RemovePostAsync(id));
		}
		[HttpPost]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Add(PostAddDto dto)
		{
			return ActionResultInstance(await _postService.AddPostAsync(dto));
		}
		[HttpGet]
		public async Task<IActionResult> All()
		{
			return ActionResultInstance(await _postService.GetAllPostsAsync());
		}
		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return ActionResultInstance(await _postService.GetSinglePostAsync(id));
		}
		[HttpPut]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> Update(PostUpdateDto dto)
		{
			return ActionResultInstance(await _postService.UpdatePostAsync(dto));
		}
		[HttpGet]
		public async Task<IActionResult> GetActive()
		{
			return ActionResultInstance(await _postService.GetActiveOrNotActivePostsAsync(true));
		}
		[HttpPost]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> AddComment(CommentAddDto dto)
		{
			return ActionResultInstance(await _postService.AddCommentAsync(dto));
		}
		[HttpGet]
		[Route("{id}")]
		[Authorize]
		[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
		public async Task<IActionResult> AddLike(Guid id)
		{
			return ActionResultInstance(await _postService.AddLikeAsync(id));
		}

	}
}
