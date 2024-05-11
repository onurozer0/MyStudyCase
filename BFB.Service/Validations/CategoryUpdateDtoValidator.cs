using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
	{
		public CategoryUpdateDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().WithMessage("Kategori adı alanı boş bırakılamaz!");
			RuleFor(x => x.ParentId).NotEqual(x => x.Id).WithMessage("Kategori kendi üst kategorisi olamaz!");
		}
	}
}
