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
	public class PostService : Service<Post>, IPostService
	{
		private readonly IGenericRepository<PostComment> _commentRepository;
		private readonly IGenericRepository<PostLike> _likeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IGenericRepository<Post> _repository;
		public PostService(IUnitOfWork unitOfWork, IGenericRepository<Post> repository, IGenericRepository<PostComment> commentRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IGenericRepository<PostLike> likeRepository) : base(unitOfWork, repository)
		{
			_unitOfWork = unitOfWork;
			_commentRepository = commentRepository;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_likeRepository = likeRepository;
			_repository = repository;
		}

		public async Task<Response<CommentsDto>> AddCommentAsync(CommentAddDto commentAddDto)
		{
			var post = await GetByIdAsync(commentAddDto.ItemId);
			if (post == null)
			{
				return Response<CommentsDto>.Fail("Ürün bulunamadı!", 400, true);
			}
			var comment = ObjectMapper.Mapper.Map<PostComment>(commentAddDto);
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			comment.UserId = user!.Id;
			comment.PostId = post.Id;
			await _commentRepository.AddAsync(comment);
			await _unitOfWork.CommitAsync();
			return Response<CommentsDto>.Success(ObjectMapper.Mapper.Map<CommentsDto>(comment), 200);
		}

		public async Task<Response<NoDataDto>> AddLikeAsync(Guid id)
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var isExist = await _likeRepository.Where(x => x.PostId == id && x.UserId == user!.Id).FirstOrDefaultAsync();
			if (isExist != null)
			{
				_likeRepository.Remove(isExist);
			}
			else
			{
				var like = new PostLike()
				{
					PostId = id,
					UserId = user!.Id,
				};
				await _likeRepository.AddAsync(like);
			}
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<PostDto>> AddPostAsync(PostAddDto postAddDto)
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var post = ObjectMapper.Mapper.Map<Post>(postAddDto);
			post.UserId = user!.Id;
			await AddAsync(post);
			var postDto = ObjectMapper.Mapper.Map<PostDto>(post);
			return Response<PostDto>.Success(postDto, 200);
		}

		public async Task<Response<IEnumerable<PostDto>>> GetAllPostsAsync()
		{
			var posts = await _repository.GetAll().Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).OrderByDescending(x => x.CreatedDate).ToListAsync();
			if (posts.Count == 0)
			{
				return Response<IEnumerable<PostDto>>.Fail("Posts not found.", 404, true);
			}
			var postDtos = ObjectMapper.Mapper.Map<List<PostDto>>(posts);

			return Response<IEnumerable<PostDto>>.Success(postDtos, 200);
		}
		public async Task<Response<IEnumerable<PostDto>>> GetActiveOrNotActivePostsAsync(bool isActive)
		{
			var activePosts = await Where(x => x.IsActive == isActive).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).ToListAsync();
			if (activePosts.Count == 0)
			{
				return Response<IEnumerable<PostDto>>.Fail("Posts not found.", 404, true);
			}
			var postDtos = ObjectMapper.Mapper.Map<List<PostDto>>(activePosts);

			return Response<IEnumerable<PostDto>>.Success(postDtos, 200);
		}

		public async Task<Response<SinglePostDto>> GetSinglePostAsync(Guid id)
		{
			var post = await Where(x => x.Id == id).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).Include(x => x.User).FirstAsync();
			var likeDtos = ObjectMapper.Mapper.Map<ICollection<LikesDto>>(post.Likes);
			var commentDtos = ObjectMapper.Mapper.Map<ICollection<CommentsDto>>(post.Comments);
			var singlepostDto = new SinglePostDto()
			{
				Title = post.Title,
				Content = post.Content,
				CommentsCount = post.Comments.Count,
				LikesCount = post.Likes.Count,
				User = ObjectMapper.Mapper.Map<UserDto>(post.User),
				Comments = commentDtos,
				CreatedDate = post.CreatedDate,
				Likes = likeDtos,
				Id = post.Id,
			};
			return Response<SinglePostDto>.Success(singlepostDto, 200);
		}

		public async Task<Response<NoDataDto>> RemovePostAsync(Guid id)
		{
			var post = await GetByIdAsync(id);
			post.IsActive = false;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdatePostAsync(PostUpdateDto postUpdateDto)
		{
			var post = await GetByIdAsync(postUpdateDto.Id);
			if (post == null)
			{
				return Response<NoDataDto>.Fail("Post not found.", 404, true);

			}
			if (!post.IsActive)
			{
				return Response<NoDataDto>.Fail("Post not activated.", 400, true);
			}
			post.UpdatedDate = DateTime.Now;
			post.Title = postUpdateDto.Title;
			post.Content = postUpdateDto.Content;
			post.UpdatedDate = DateTime.Now;
			await UpdateAsync(post);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> DeletePostAsync(Guid id)
		{
			var post = await GetByIdAsync(id);
			var comments = await _commentRepository.Where(x => x.PostId == id).ToListAsync();
			if (comments.Any())
			{
				_commentRepository.RemoveRange(comments);
			}
			var likes = await _likeRepository.Where(x => x.PostId == id).ToListAsync();
			if (likes.Any())
			{
				_likeRepository.RemoveRange(likes);
			}
			_repository.Remove(post);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<PostDto>>> GetMostLikedPostsAsync()
		{
			var posts = await _repository.GetAll().Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).Include(x => x.User).OrderByDescending(x => x.Likes.Count).ToListAsync();
			var postDtos = ObjectMapper.Mapper.Map<List<PostDto>>(posts);
			return Response<IEnumerable<PostDto>>.Success(postDtos, 200);
		}

		public async Task<Response<IEnumerable<PostDto>>> GetMostCommentPostsAsync()
		{
			var posts = await _repository.GetAll().Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).Include(x => x.User).OrderByDescending(x => x.Comments.Count).ToListAsync();
			var postDtos = ObjectMapper.Mapper.Map<List<PostDto>>(posts);
			return Response<IEnumerable<PostDto>>.Success(postDtos, 200);
		}
	}
}
