using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IPrivateMessageService : IService<PrivateMessage>
	{
		Task<Response<IEnumerable<PrivateMessageDto>>> GetConversationMessagesAsync(Guid conversationId);
		Task<Response<NoDataDto>> UpdateMessageAsync(UpdatePrivateMessageDto updatePrivateMessageDto);
		Task<Response<NoDataDto>> DeleteMessageAsync(Guid id);
		Task<Response<PrivateMessageDto>> SendMessageAsync(SendPrivateMessageDto sendPrivateMessageDto);
	}
}
