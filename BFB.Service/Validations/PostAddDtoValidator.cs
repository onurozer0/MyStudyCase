using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class PostAddDtoValidator : AbstractValidator<PostAddDto>
	{
		public PostAddDtoValidator()
		{
			RuleFor(x => x.Title).NotNull().WithMessage("Başlık alanı boş bırakılamaz!").MaximumLength(100);
			RuleFor(x => x.Content).NotNull().WithMessage("İçerik alanı boş bırakılamaz!").MaximumLength(2000);
		}
	}
}
