using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
	{
		public CategoryAddDtoValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı alanı boş bırakılamaz!");
		}

	}
}
