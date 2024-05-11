using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BFB.Service.Services
{
	public class CommentService : ICommentService
	{
		private readonly IService<PostComment> _postCommentService;
		private readonly IService<Comment> _commentService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly UserManager<AppUser> _userManager;

		public CommentService(IService<Comment> commentService, IService<PostComment> postCommentService, IUnitOfWork unitOfWork, IGenericRepository<Product> productRepository, UserManager<AppUser> userManager)
		{
			_commentService = commentService;
			_postCommentService = postCommentService;
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userManager = userManager;
		}

		public async Task<Response<NoDataDto>> DeleteCommentAsync(Guid id)
		{
			var comment = await _commentService.GetByIdAsync(id);
			await _commentService.RemoveAsync(comment);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> DeletePostCommentAsync(Guid id)
		{
			var comment = await _postCommentService.GetByIdAsync(id);
			await _postCommentService.RemoveAsync(comment);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<CommentsDto>>> GetCommentsByProductIdAsync(Guid productId)
		{
			var comments = await _commentService.Where(x => x.ProductId == productId).ToListAsync();
			var commentDtos = ObjectMapper.Mapper.Map<List<CommentsDto>>(comments);
			return Response<IEnumerable<CommentsDto>>.Success(commentDtos, 200);
		}

		public async Task<Response<IEnumerable<CommentsDto>>> GetCommentsByUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return Response<IEnumerable<CommentsDto>>.Fail("Kullanıcı bulunamadı!", 404, true);
			}
			var comments = await _commentService.Where(x => x.UserId == userId).ToListAsync();
			var postComments = await _postCommentService.Where(x => x.UserId == userId).ToListAsync();
			var postCommentDtos = ObjectMapper.Mapper.Map<List<CommentsDto>>(postComments);
			var commentDtos = ObjectMapper.Mapper.Map<List<CommentsDto>>(comments);
			var list = postCommentDtos.Concat(commentDtos).ToList();
			return Response<IEnumerable<CommentsDto>>.Success(list, 200);
		}

		public async Task<Response<IEnumerable<CommentsDto>>> GetPostCommentsByIdAsync(Guid postId)
		{
			var comments = await _postCommentService.Where(x => x.PostId == postId).ToListAsync();
			var commentDtos = ObjectMapper.Mapper.Map<List<CommentsDto>>(comments);
			return Response<IEnumerable<CommentsDto>>.Success(commentDtos, 200);
		}

		public async Task<Response<NoDataDto>> RemoveCommentAsync(Guid id)
		{
			var comment = await _commentService.GetByIdAsync(id);
			comment.IsDeleted = true;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> RemovePostCommentAsync(Guid id)
		{
			var comment = await _postCommentService.GetByIdAsync(id);
			comment.IsDeleted = true;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdateCommentAsync(CommentUpdateDto commentUpdateDto)
		{
			var comment = await _commentService.GetByIdAsync(commentUpdateDto.Id);
			comment.Title = commentUpdateDto.Title;
			comment.Content = commentUpdateDto.Content;
			comment.UpdatedDate = DateTime.Now;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdatePostCommentAsync(CommentUpdateDto commentUpdateDto)
		{
			var comment = await _postCommentService.GetByIdAsync(commentUpdateDto.Id);
			comment.Title = commentUpdateDto.Title;
			comment.Content = commentUpdateDto.Content;
			comment.UpdatedDate = DateTime.Now;
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}
	}
}
