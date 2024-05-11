using BFB.Core.DTOs;
using BFB.Core.Entities;
using SharedLib.DTOs;

namespace BFB.Core.Services
{
	public interface IProductService : IService<Product>
	{
		Task<Response<IEnumerable<ProductDto>>> GetActiveProductsAsync();
		Task<Response<IEnumerable<ProductDto>>> GetNotActiveProductsAsync();
		Task<Response<NoDataDto>> RemoveProductAsync(Guid id);
		Task<Response<NoDataDto>> ActivateProductAsync(Guid id);
		Task<Response<NoDataDto>> DeactivateProductAsync(Guid id);
		Task<Response<NoDataDto>> UpdateProductAsync(ProductUpdateDto productUpdateDto);
		Task<Response<SingleProductDto>> GetProductDetailsAsync(Guid id);
		Task<Response<ProductDto>> AddProductAsync(ProductAddDto productAddDto);
		Task<Response<CommentsDto>> AddCommentAsync(CommentAddDto commentAddDto);
		Task<Response<List<ProductDto>>> GetProductsByUserIdAsync(string userId);
		Task<Response<NoDataDto>> AddLikeAsync(Guid id);
		Task<Response<NoDataDto>> DeleteProductAsync(Guid id);
		Task<Response<IEnumerable<ProductDto>>> GetMostCommentProductsAsync();
		Task<Response<IEnumerable<ProductDto>>> GetMostLikedProductsAsync();

	}
}
