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

	public class ConversationController : CustomControllerBase
	{
		private readonly IPrivateMessageService _privateMessageService;

		private readonly IConversationService _conversationService;

		public ConversationController(IConversationService conversationService, IPrivateMessageService privateMessageService)
		{
			_conversationService = conversationService;
			_privateMessageService = privateMessageService;
		}
		[HttpGet]
		public async Task<IActionResult> MyConversations()
		{
			return ActionResultInstance(await _conversationService.GetMyConversationsAsync());
		}
		[HttpGet]
		[Route("{id}")]
		[ServiceFilter(typeof(NotFoundFilter<Conversation>))]
		public async Task<IActionResult> Get(Guid id)
		{
			return ActionResultInstance(await _conversationService.GetConversationByIdAsync(id));
		}
		[HttpPost]
		public async Task<IActionResult> Start(StartConversationDto dto)
		{
			return ActionResultInstance(await _conversationService.StartConversationAsync(dto));
		}
		[HttpPost]
		public async Task<IActionResult> Send(SendPrivateMessageDto dto)
		{
			return ActionResultInstance(await _privateMessageService.SendMessageAsync(dto));
		}
		[HttpPut]
		public async Task<IActionResult> Update(UpdatePrivateMessageDto dto)
		{
			return ActionResultInstance(await _privateMessageService.UpdateMessageAsync(dto));
		}
		[HttpDelete]
		[ServiceFilter(typeof(NotFoundFilter<PrivateMessage>))]
		[Route("{id}")]
		public async Task<IActionResult> DeleteMsg(Guid id)
		{
			return ActionResultInstance(await _privateMessageService.DeleteMessageAsync(id));
		}
	}
}
