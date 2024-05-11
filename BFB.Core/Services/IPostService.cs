using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IPostService : IService<Post>
	{
		Task<Response<NoDataDto>> RemovePostAsync(Guid id);
		Task<Response<NoDataDto>> DeletePostAsync(Guid id);
		Task<Response<PostDto>> AddPostAsync(PostAddDto postAddDto);
		Task<Response<IEnumerable<PostDto>>> GetAllPostsAsync();
		Task<Response<SinglePostDto>> GetSinglePostAsync(Guid id);
		Task<Response<NoDataDto>> UpdatePostAsync(PostUpdateDto postUpdateDto);
		Task<Response<IEnumerable<PostDto>>> GetActiveOrNotActivePostsAsync(bool isActive);
		Task<Response<CommentsDto>> AddCommentAsync(CommentAddDto commentAddDto);
		Task<Response<NoDataDto>> AddLikeAsync(Guid id);
		Task<Response<IEnumerable<PostDto>>> GetMostLikedPostsAsync();
		Task<Response<IEnumerable<PostDto>>> GetMostCommentPostsAsync();

	}
}
