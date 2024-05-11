using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IConversationService : IService<Conversation>
	{
		Task<Response<IEnumerable<ConversationDto>>> GetMyConversationsAsync();
		Task<Response<IEnumerable<ConversationDto>>> GetUserConversationsAsync(string userId);
		Task<Response<ConversationDto>> GetConversationByIdAsync(Guid id);
		Task<Response<ConversationDto>> StartConversationAsync(StartConversationDto startConversationDto);
		Task<Response<NoDataDto>> DeleteConversationAsync(Guid id);
	}
}
