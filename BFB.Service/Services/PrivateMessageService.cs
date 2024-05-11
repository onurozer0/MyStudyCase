using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BFB.Service.Services
{
	public class PrivateMessageService : Service<PrivateMessage>, IPrivateMessageService
	{
		private readonly IGenericRepository<Conversation> _conversationRepository;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IUnitOfWork _unitOfWork;
		public PrivateMessageService(IUnitOfWork unitOfWork, IGenericRepository<PrivateMessage> repository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IGenericRepository<Conversation> conversationRepository) : base(unitOfWork, repository)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
			_conversationRepository = conversationRepository;
		}

		public async Task<Response<NoDataDto>> DeleteMessageAsync(Guid id)
		{
			await RemoveAsync(await GetByIdAsync(id));
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<PrivateMessageDto>>> GetConversationMessagesAsync(Guid conversationId)
		{
			var conversation = await _conversationRepository.GetByIdAsync(conversationId);
			if (conversation == null)
			{
				return Response<IEnumerable<PrivateMessageDto>>.Fail("Conversation not found", 404, false);
			}
			var messages = await Where(x => x.ConversationId == conversationId).ToListAsync();
			var messagesDto = ObjectMapper.Mapper.Map<ICollection<PrivateMessageDto>>(messages);
			return Response<IEnumerable<PrivateMessageDto>>.Success(messagesDto, 200);
		}

		public async Task<Response<PrivateMessageDto>> SendMessageAsync(SendPrivateMessageDto sendPrivateMessageDto)
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var conversation = await _conversationRepository.GetByIdAsync(sendPrivateMessageDto.ConversationId);
			if (conversation == null)
			{
				return Response<PrivateMessageDto>.Fail("Conversation not found", 400, false);
			}
			var receiverId = string.Empty;
			if (conversation.StarterId == user!.Id)
			{
				receiverId = conversation.ReceiverId;
			}
			else
			{
				receiverId = conversation.StarterId;
			}
			var message = new PrivateMessage()
			{
				SenderId = user!.Id,
				ConversationId = sendPrivateMessageDto.ConversationId,
				Message = sendPrivateMessageDto.Message,
				ReceiverId = receiverId,
			};
			await AddAsync(message);
			var messageDto = ObjectMapper.Mapper.Map<PrivateMessageDto>(message);
			return Response<PrivateMessageDto>.Success(messageDto, 200);

		}

		public async Task<Response<NoDataDto>> UpdateMessageAsync(UpdatePrivateMessageDto updatePrivateMessageDto)
		{
			var message = await GetByIdAsync(updatePrivateMessageDto.MessageId);
			if (message == null)
			{
				return Response<NoDataDto>.Fail("Mesaj bulunamadı!", 404, true);
			}
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);

			if (message.SenderId != user!.Id)
			{
				return Response<NoDataDto>.Fail("", 403, false);
			}
			message.Message = updatePrivateMessageDto.Content;
			message.UpdatedDate = DateTime.UtcNow;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(200);
		}
	}
}
