using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class CommentAddDtoValidator : AbstractValidator<CommentAddDto>
	{
		public CommentAddDtoValidator()
		{
			RuleFor(x => x.Title).NotNull().WithMessage("Yorum başlığı boş bırakılamaz!").MaximumLength(100);
			RuleFor(x => x.Content).NotNull().WithMessage("Yorum içeriği boş bırakılamaz!").MaximumLength(2000);
			RuleFor(x => x.ItemId).NotNull();
		}
	}
}
