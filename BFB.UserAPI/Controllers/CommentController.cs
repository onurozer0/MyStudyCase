using BFB.AdminAPI.Controllers;
using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Services;
using BFB.UserAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFB.UserAPI.Controllers
{
	[Authorize]
	[ServiceFilter(typeof(IsCreatedProfileFilterAttribute))]
	public class CommentController : CustomControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}
		[Route("{id}")]
		[HttpDelete]
		[ServiceFilter(typeof(NotFoundFilter<Comment>))]

		public async Task<IActionResult> Remove(Guid id)
		{
			return ActionResultInstance(await _commentService.RemoveCommentAsync(id));
		}
		[Route("/api/comment/removepost/{id}")]
		[HttpDelete]
		[ServiceFilter(typeof(NotFoundFilter<PostComment>))]
		public async Task<IActionResult> RemovePost(Guid id)
		{
			return ActionResultInstance(await _commentService.RemovePostCommentAsync(id));
		}
		[HttpPut]
		[ServiceFilter(typeof(NotFoundFilter<Comment>))]
		public async Task<IActionResult> Update(CommentUpdateDto dto)
		{
			return ActionResultInstance(await _commentService.UpdateCommentAsync(dto));
		}
		[HttpPut]
		[ServiceFilter(typeof(NotFoundFilter<PostComment>))]
		public async Task<IActionResult> UpdatePost(CommentUpdateDto dto)
		{
			return ActionResultInstance(await _commentService.UpdatePostCommentAsync(dto));
		}
	}
}
