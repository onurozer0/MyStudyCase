using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class StartConversationDtoValidator : AbstractValidator<StartConversationDto>
	{
		public StartConversationDtoValidator()
		{
			RuleFor(x => x.Message).NotNull().WithMessage("Mesaj alanı boş bırakılamaz!").MaximumLength(2000).WithMessage("Maksimum 2000 karakter uzunluğunda mesaj gönderilebilir!");
		}
	}
}
