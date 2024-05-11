using BFB.Core.DTOs;
using SharedLib.DTOs;


namespace BFB.Core.Services
{
	public interface ICommentService
	{
		Task<Response<NoDataDto>> UpdateCommentAsync(CommentUpdateDto commentUpdateDto);
		Task<Response<IEnumerable<CommentsDto>>> GetPostCommentsByIdAsync(Guid id);
		Task<Response<IEnumerable<CommentsDto>>> GetCommentsByProductIdAsync(Guid id);
		Task<Response<IEnumerable<CommentsDto>>> GetCommentsByUserAsync(string userId);
		Task<Response<NoDataDto>> RemoveCommentAsync(Guid id);
		Task<Response<NoDataDto>> DeleteCommentAsync(Guid id);
		Task<Response<NoDataDto>> UpdatePostCommentAsync(CommentUpdateDto commentUpdateDto);
		Task<Response<NoDataDto>> RemovePostCommentAsync(Guid id);
		Task<Response<NoDataDto>> DeletePostCommentAsync(Guid id);
	}
}
