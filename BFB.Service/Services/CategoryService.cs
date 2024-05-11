using BFB.Core.DTOs;
using BFB.Core.Entities;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Service.MapProfile;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BFB.Service.Services
{
	public class CategoryService : Service<Category>, ICategoryService
	{
		private readonly IGenericRepository<Category> _repository;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IGenericRepository<Like> _likeRepository;
		private readonly IGenericRepository<Comment> _commentRepository;
		private readonly IUnitOfWork _unitOfWork;
		public CategoryService(IUnitOfWork unitOfWork, IGenericRepository<Category> repository, IGenericRepository<Product> productRepository, IGenericRepository<Like> likeRepository, IGenericRepository<Comment> commentRepository) : base(unitOfWork, repository)
		{
			_repository = repository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
			_likeRepository = likeRepository;
			_commentRepository = commentRepository;
		}

		public async Task<Response<CategoryDto>> AddCategoryAsync(CategoryAddDto categoryAddDto)
		{

			if (categoryAddDto.ParentId != null)
			{
				var id = new Guid();
				try
				{
					id = Guid.Parse(categoryAddDto.ParentId);
				}
				catch (Exception)
				{
					return Response<CategoryDto>.Fail("ParentId formatı doğru değil.", 400, true);
				}
				var possibleParent = await _repository.GetByIdAsync(id);
				if (possibleParent == null)
				{
					return Response<CategoryDto>.Fail("Üst kategori bulunamadı!", 400, true);
				}
				if (possibleParent.ParentID != Guid.Empty)
				{
					return Response<CategoryDto>.Fail("Üst kategori bir alt kategori olamaz!", 400, true);
				}
			}
			var category = ObjectMapper.Mapper.Map<Category>(categoryAddDto);
			await AddAsync(category);
			var categoryDto = ObjectMapper.Mapper.Map<CategoryDto>(category);
			return Response<CategoryDto>.Success(categoryDto, 200);
		}

		public async Task<Response<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
		{
			var categories = await _repository.GetAll().Include(x => x.SubCategories).Include(x => x.ParentCategory).ToListAsync();
			if (categories.Count == 0)
			{
				return Response<IEnumerable<CategoryDto>>.Fail("Kategori bulunamadı!", 404, true);
			}
			var categoryDtos = new List<CategoryDto>();
			foreach (var category in categories)
			{
				if (category.ParentID == null)
				{
					var categoryDto = new CategoryDto()
					{
						SubCategories = ObjectMapper.Mapper.Map<List<CategoryDto>>(category.SubCategories),
						Id = category.Id,
						Name = category.Name,
						ParentID = category.ParentID,
					};
					categoryDtos.Add(categoryDto);
				}

			}
			return Response<IEnumerable<CategoryDto>>.Success(categoryDtos, 200);
		}

		public async Task<Response<CategoryDto>> GetCategoryByIdAsync(Guid id)
		{
			var category = await Where(x => x.Id == id).Include(x => x.SubCategories).FirstAsync();
			var categoryDto = ObjectMapper.Mapper.Map<CategoryDto>(category);
			return Response<CategoryDto>.Success(categoryDto, 200);
		}

		public async Task<Response<CategoryWithProductsDto>> GetCategoryWithProductsAsync(Guid id)
		{

			var category = await Where(x => x.Id == id).FirstAsync();
			var categoryWithProductsDto = new CategoryWithProductsDto
			{
				Category = ObjectMapper.Mapper.Map<CategoryDto>(category),
			};
			var products = await _productRepository.Where(x => x.CategoryId == id).Include(x => x.User).Include(x => x.Likes).Include(x => x.Comments).ToListAsync();

			if (products != null)
			{
				categoryWithProductsDto.Products = products.Select(x => new ProductDto()
				{
					Name = x.Name,
					CommentsCount = x.Comments.Count,
					LikesCount = x.Likes.Count,
					CreatedDate = x.CreatedDate,
					Description = x.Description,
					Id = x.Id,
					User = new UserDto() { Id = x.User.Id, Surname = x.User.Surname!, Name = x.User.Name!, UserName = x.User.UserName! }
				}).ToList();
			}
			return Response<CategoryWithProductsDto>.Success(categoryWithProductsDto, 200);
		}

		public async Task<Response<IEnumerable<CategoryDto>>> GetParentCategoriesAsync()
		{
			var categories = await Where(x => x.ParentID == null).ToListAsync();
			var categoryDtos = ObjectMapper.Mapper.Map<IEnumerable<CategoryDto>>(categories);
			return Response<IEnumerable<CategoryDto>>.Success(categoryDtos, 200);
		}

		public async Task<Response<IEnumerable<CategoryDto>>> GetSubCategoriesAsync()
		{
			var categories = await Where(x => x.ParentID != null).ToListAsync();
			var categoryDtos = ObjectMapper.Mapper.Map<IEnumerable<CategoryDto>>(categories);
			return Response<IEnumerable<CategoryDto>>.Success(categoryDtos, 200);
		}

		public async Task<Response<NoDataDto>> DeleteCategoryAsync(Guid id)
		{
			var category = await Where(x => x.Id == id).Include(x => x.SubCategories).AsNoTracking().FirstAsync();
			var products = await _productRepository.Where(x => x.CategoryId == id).Include(x => x.Comments).Include(x => x.Likes).AsNoTracking().ToListAsync();
			if (products != null)
			{
				foreach (var product in products)
				{
					if (product.Comments != null)
					{
						_commentRepository.RemoveRange(product.Comments);
					}
					if (product.Likes != null)
					{
						_likeRepository.RemoveRange(product.Likes);
					}
				}
			}
			if (category.SubCategories != null)
			{
				foreach (var subCategory in category.SubCategories)
				{
					subCategory.ParentID = null;
				}
			}

			_repository.Remove(category);
			await _unitOfWork.CommitAsync();
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
		{
			var category = await Where(x => x.Id == categoryUpdateDto.Id).AsNoTracking().FirstAsync();
			if (category == null)
			{
				return Response<NoDataDto>.Fail("Kategori bulunamadı!", 404, true);
			}
			if (categoryUpdateDto.ParentId != Guid.Empty)
			{
				var parentCategory = await GetByIdAsync(categoryUpdateDto.ParentId);
				if (parentCategory == null)
				{
					return Response<NoDataDto>.Fail("Üst kategori bulunamadı!", 400, true);
				}
				if (parentCategory.ParentID != null)
				{
					return Response<NoDataDto>.Fail("Seçilen kategori bir üst kategori değil!", 400, true);
				}
			}
			category = ObjectMapper.Mapper.Map<Category>(categoryUpdateDto);
			if (category.ParentID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
			{
				category.ParentID = null;
			}
			category.UpdatedDate = DateTime.Now;
			await UpdateAsync(category);
			return Response<NoDataDto>.Success(204);

		}
	}
}
