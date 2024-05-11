using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
	{
		public ProductUpdateDtoValidator()
		{
			RuleFor(x => x.Description).NotNull().WithMessage("Açıklama alanı boş bırakılamaz!");
			RuleFor(x => x.Name).NotNull().WithMessage("Ürün ismi alanı boş bırakılamaz!");
			RuleFor(x => x.CategoryId).NotNull().WithMessage("Kategori seçimi yapınız!");
		}
	}
}
