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
	public class ConversationService : Service<Conversation>, IConversationService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IPrivateMessageService _privateMessageService;
		public ConversationService(IUnitOfWork unitOfWork, IGenericRepository<Conversation> repository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IPrivateMessageService privateMessageService) : base(unitOfWork, repository)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_privateMessageService = privateMessageService;
		}

		public async Task<Response<NoDataDto>> DeleteConversationAsync(Guid id)
		{
			var messages = await _privateMessageService.Where(x => x.ConversationId == id).ToListAsync();
			await _privateMessageService.RemoveRangeAsync(messages);
			var conversation = await GetByIdAsync(id);
			await RemoveAsync(conversation);
			return Response<NoDataDto>.Success(204);

		}

		public async Task<Response<ConversationDto>> GetConversationByIdAsync(Guid id)
		{
			var conversation = await Where(x => x.Id == id).Include(x => x.Messages).Include(x => x.Starter).Include(x => x.Receiver).FirstOrDefaultAsync();
			var conversationDto = ObjectMapper.Mapper.Map<ConversationDto>(conversation);
			return Response<ConversationDto>.Success(conversationDto, 200);
		}

		public async Task<Response<IEnumerable<ConversationDto>>> GetMyConversationsAsync()
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var conversations = await Where(x => x.StarterId == user!.Id || x.ReceiverId == user.Id).Include(x => x.Messages).Include(x => x.Receiver).Include(x => x.Starter).ToListAsync();
			var conversationDtos = ObjectMapper.Mapper.Map<List<ConversationDto>>(conversations);
			return Response<IEnumerable<ConversationDto>>.Success(conversationDtos, 200);
		}

		public async Task<Response<IEnumerable<ConversationDto>>> GetUserConversationsAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<IEnumerable<ConversationDto>>.Fail("Kullanıcı bulunamadı!", 404, true);
			}
			var conversations = await Where(x => x.StarterId == userId || x.ReceiverId == userId).Include(x => x.Messages).Include(x => x.Receiver).Include(x => x.Starter).ToListAsync();
			var conversationDtos = ObjectMapper.Mapper.Map<List<ConversationDto>>(conversations);
			return Response<IEnumerable<ConversationDto>>.Success(conversationDtos, 200);
		}

		public async Task<Response<ConversationDto>> StartConversationAsync(StartConversationDto startConversationDto)
		{

			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			if (user.Id == startConversationDto.ReceiverId)
			{
				return Response<ConversationDto>.Fail("Kendinize Mesaj Gönderemezsiniz!", 400, true);
			}
			var receiver = await _userManager.FindByIdAsync(startConversationDto.ReceiverId);
			if (receiver == null)
			{
				return Response<ConversationDto>.Fail("Kullanıcı bulunamadı!", 400, true);
			}
			var conversation = new Conversation()
			{
				StarterId = user!.Id,
				ReceiverId = receiver.Id,
				Title = startConversationDto.Title,
			};
			await AddAsync(conversation);
			var message = new PrivateMessage()
			{
				SenderId = user.Id,
				ReceiverId = receiver.Id,
				ConversationId = conversation.Id,
				Message = startConversationDto.Message
			};
			await _privateMessageService.AddAsync(message);
			var conversationDto = ObjectMapper.Mapper.Map<ConversationDto>(conversation);
			return Response<ConversationDto>.Success(conversationDto, 200);

		}
	}
}
