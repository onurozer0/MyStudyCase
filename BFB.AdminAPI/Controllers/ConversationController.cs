using BFB.AdminAPI.Filters;
using BFB.Core.Entities;
using BFB.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BFB.AdminAPI.Controllers
{
	public class ConversationController : CustomControllerBase
	{
		private readonly IConversationService _conversationService;

		public ConversationController(IConversationService conversationService)
		{
			_conversationService = conversationService;
		}

		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> Users(string userId)
		{
			return ActionResultInstance(await _conversationService.GetUserConversationsAsync(userId));
		}
		[HttpGet]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Conversation>))]
		public async Task<IActionResult> Get(Guid id)
		{
			return ActionResultInstance(await _conversationService.GetConversationByIdAsync(id));
		}
		[HttpDelete]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Conversation>))]
		public async Task<IActionResult> Delete(Guid id)
		{
			return ActionResultInstance(await _conversationService.DeleteConversationAsync(id));
		}
	}
}
