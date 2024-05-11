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
	public class ProductService : Service<Product>, IProductService
	{
		private readonly IGenericRepository<Comment> _commentRepository;
		private readonly IGenericRepository<Like> _likeRepository;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IService<Category> _categoryService;

		public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IGenericRepository<Comment> commentRepository, IGenericRepository<Like> likeRepository, IGenericRepository<Product> productRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IService<Category> categoryService) : base(unitOfWork, repository)
		{
			_commentRepository = commentRepository;
			_likeRepository = likeRepository;
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_categoryService = categoryService;
		}

		public async Task<Response<NoDataDto>> ActivateProductAsync(Guid id)
		{
			var product = await GetByIdAsync(id);
			product.IsActive = true;
			await UpdateAsync(product);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<CommentsDto>> AddCommentAsync(CommentAddDto commentAddDto)
		{
			var product = await GetByIdAsync(commentAddDto.ItemId);
			if (product == null)
			{
				return Response<CommentsDto>.Fail("Ürün bulunamadı!", 400, true);
			}
			var comment = ObjectMapper.Mapper.Map<Comment>(commentAddDto);
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			comment.UserId = user!.Id;
			await _commentRepository.AddAsync(comment);
			await _unitOfWork.CommitAsync();
			return Response<CommentsDto>.Success(ObjectMapper.Mapper.Map<CommentsDto>(comment), 200);
		}

		public async Task<Response<NoDataDto>> AddLikeAsync(Guid id)
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var isExist = await _likeRepository.Where(x => x.ProductId == id && x.UserId == user!.Id).FirstOrDefaultAsync();
			if (isExist != null)
			{
				_likeRepository.Remove(isExist);
			}
			else
			{
				var like = new Like()
				{
					ProductId = id,
					UserId = user!.Id,
				};
				await _likeRepository.AddAsync(like);
			}
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<ProductDto>> AddProductAsync(ProductAddDto productAddDto)
		{
			var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext!.User.Identity!.Name!);
			var category = await _categoryService.GetByIdAsync(productAddDto.CategoryId);
			if (category == null)
			{
				return Response<ProductDto>.Fail("Kategori bulunamadı!", 400, true);
			}
			var product = new Product()
			{
				CategoryId = productAddDto.CategoryId,
				CreatedDate = DateTime.Now,
				Description = productAddDto.Description,
				Name = productAddDto.Name,
				UserId = user!.Id
			};
			await AddAsync(product);
			var productDto = ObjectMapper.Mapper.Map<ProductDto>(product);
			productDto.Category.Name = category.Name;
			return Response<ProductDto>.Success(productDto, 200);
		}

		public async Task<Response<NoDataDto>> DeactivateProductAsync(Guid id)
		{
			var product = await GetByIdAsync(id);
			product.IsActive = true;
			await UpdateAsync(product);
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> DeleteProductAsync(Guid id)
		{
			var product = await GetByIdAsync(id);
			var comments = await _commentRepository.Where(x => x.ProductId == id).ToListAsync();
			var likes = await _likeRepository.Where(x => x.ProductId == id).ToListAsync();
			_commentRepository.RemoveRange(comments);
			_likeRepository.RemoveRange(likes);
			_productRepository.Remove(product);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<ProductDto>>> GetActiveProductsAsync()
		{
			var activeProducts = await Where(x => x.IsActive == true).Include(x => x.Category).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).OrderByDescending(x => x.CreatedDate
			).ToListAsync();
			if (activeProducts.Count == 0)
			{
				return Response<IEnumerable<ProductDto>>.Fail("Products not found.", 404, true);
			}
			var productDtos = new List<ProductDto>();
			foreach (var product in activeProducts)
			{
				var productDto = new ProductDto()
				{
					Category = new() { Id = product.CategoryId, Name = product.Category.Name },
					CommentsCount = product.Comments.Count,
					LikesCount = product.Likes.Count,
					CreatedDate = product.CreatedDate,
					Description = product.Description,
					Id = product.Id,
					Name = product.Name,
					User = new() { Id = product.UserId, UserName = product.User.UserName!, Name = product.User.Name!, Surname = product.User.Surname! }
				};
				productDtos.Add(productDto);
			}
			return Response<IEnumerable<ProductDto>>.Success(productDtos, 200);
		}

		public async Task<Response<IEnumerable<ProductDto>>> GetMostCommentProductsAsync()
		{
			var products = await Where(x => x.IsActive == true).Include(x => x.Category).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).OrderByDescending(x => x.Comments.Count
			).ToListAsync();
			if (products.Count == 0)
			{
				return Response<IEnumerable<ProductDto>>.Fail("Products not found.", 404, true);
			}
			var productDtos = new List<ProductDto>();
			foreach (var product in products)
			{
				var productDto = new ProductDto()
				{
					Category = new() { Id = product.CategoryId, Name = product.Category.Name },
					CommentsCount = product.Comments.Count,
					LikesCount = product.Likes.Count,
					CreatedDate = product.CreatedDate,
					Description = product.Description,
					Id = product.Id,
					Name = product.Name,
					User = new() { Id = product.UserId, UserName = product.User.UserName!, Name = product.User.Name!, Surname = product.User.Surname! }
				};
				productDtos.Add(productDto);
			}
			return Response<IEnumerable<ProductDto>>.Success(productDtos, 200);

		}

		public async Task<Response<IEnumerable<ProductDto>>> GetMostLikedProductsAsync()
		{
			var products = await Where(x => x.IsActive == true).Include(x => x.Category).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).OrderByDescending(x => x.Likes.Count
			).ToListAsync();
			if (products.Count == 0)
			{
				return Response<IEnumerable<ProductDto>>.Fail("Products not found.", 404, true);
			}
			var productDtos = new List<ProductDto>();
			foreach (var product in products)
			{
				var productDto = new ProductDto()
				{
					Category = new() { Id = product.CategoryId, Name = product.Category.Name },
					CommentsCount = product.Comments.Count,
					LikesCount = product.Likes.Count,
					CreatedDate = product.CreatedDate,
					Description = product.Description,
					Id = product.Id,
					Name = product.Name,
					User = new() { Id = product.UserId, UserName = product.User.UserName!, Name = product.User.Name!, Surname = product.User.Surname! }
				};
				productDtos.Add(productDto);
			}
			return Response<IEnumerable<ProductDto>>.Success(productDtos, 200);
		}

		public async Task<Response<IEnumerable<ProductDto>>> GetNotActiveProductsAsync()
		{
			var notActiveProducts = await Where(x => x.IsActive == false).Include(x => x.Category).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments).OrderByDescending(x => x.CreatedDate).ToListAsync();
			if (notActiveProducts.Count == 0)
			{
				return Response<IEnumerable<ProductDto>>.Fail("Products not found.", 404, true);
			}
			var productDtos = new List<ProductDto>();
			foreach (var product in notActiveProducts)
			{
				var productDto = new ProductDto()
				{
					Category = new() { Id = product.CategoryId, Name = product.Category.Name },
					CommentsCount = product.Comments.Count,
					LikesCount = product.Likes.Count,
					CreatedDate = product.CreatedDate,
					Description = product.Description,
					Id = product.Id,
					Name = product.Name,
					User = new() { Id = product.UserId, UserName = product.User.UserName!, Name = product.User.Name!, Surname = product.User.Surname! }
				};
				productDtos.Add(productDto);
			}
			return Response<IEnumerable<ProductDto>>.Success(productDtos, 200);
		}

		public async Task<Response<SingleProductDto>> GetProductDetailsAsync(Guid id)
		{
			var product = await Where(x => x.Id == id).Include(x => x.Category).Include(x => x.Likes).Include(x => x.Comments.Where(x => x.IsDeleted == false)).Include(x => x.User).FirstAsync();
			var likeDtos = new List<LikesDto>();
			if (product.Likes.Count > 0)
			{
				foreach (var like in product.Likes)
				{
					var likeDto = new LikesDto()
					{
						Id = like.Id,
						User = ObjectMapper.Mapper.Map<UserDto>(like.User)
					};
					likeDtos.Add(likeDto);
				}
			}
			var commentDtos = new List<CommentsDto>();
			if (product.Comments.Count > 0)
			{
				foreach (var comment in product.Comments)
				{
					var commentDto = new CommentsDto()
					{
						Id = comment.Id,
						Content = comment.Content,
						CreatedDate = comment.CreatedDate,
						Title = comment.Title,
						User = ObjectMapper.Mapper.Map<UserDto>(comment.User)
					};
					commentDtos.Add(commentDto);
				}
			}
			var singleProductDto = new SingleProductDto()
			{
				Name = product.Name,
				Description = product.Description,
				CommentsCount = product.Comments.Count,
				LikesCount = product.Likes.Count,
				User = ObjectMapper.Mapper.Map<UserDto>(product.User),
				Category = ObjectMapper.Mapper.Map<CategoryDto>(product.Category),
				Comments = commentDtos,
				CreatedDate = product.CreatedDate,
				Likes = likeDtos,
				Id = product.Id,
			};
			return Response<SingleProductDto>.Success(singleProductDto, 200);
		}

		public async Task<Response<NoDataDto>> RemoveProductAsync(Guid id)
		{
			var product = await GetByIdAsync(id);
			var comments = await _commentRepository.Where(x => x.ProductId == id).ToListAsync();
			if (comments.Any())
			{
				_commentRepository.RemoveRange(comments);
			}
			var likes = await _likeRepository.Where(x => x.ProductId == id).ToListAsync();
			if (likes.Any())
			{
				_likeRepository.RemoveRange(likes);
			}
			_productRepository.Remove(product);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdateProductAsync(ProductUpdateDto productUpdateDto)
		{
			var product = await GetByIdAsync(productUpdateDto.Id);
			var category = _categoryService.GetByIdAsync(productUpdateDto.CategoryId);
			if (category == null)
			{
				return Response<NoDataDto>.Fail("Category not found.", 404, true);
			}
			if (product == null)
			{
				return Response<NoDataDto>.Fail("Product not found.", 404, true);

			}
			if (!product.IsActive)
			{
				return Response<NoDataDto>.Fail("Product not activated.", 400, true);
			}
			product.UpdatedDate = DateTime.Now;
			product.Name = productUpdateDto.Name;
			product.CategoryId = productUpdateDto.CategoryId;
			product.Description = productUpdateDto.Description;
			product.UpdatedDate = DateTime.Now;
			await UpdateAsync(product);
			return Response<NoDataDto>.Success(204);
		}
		public async Task<Response<List<ProductDto>>> GetProductsByUserIdAsync(string userId)
		{

			var products = await Where(x => x.UserId == userId).ToListAsync();
			var productsDto = ObjectMapper.Mapper.Map<List<ProductDto>>(products);
			return Response<List<ProductDto>>.Success(productsDto, 200);
		}
	}
}
