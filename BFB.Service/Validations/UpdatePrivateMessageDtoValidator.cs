using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class UpdatePrivateMessageDtoValidator : AbstractValidator<UpdatePrivateMessageDto>
	{
		public UpdatePrivateMessageDtoValidator()
		{
			RuleFor(x => x.Content).NotNull().WithMessage("Mesaj alanı boş bırakılamaz!").MaximumLength(2000).WithMessage("Maksimum 2000 karakter uzunluğunda mesaj gönderilebilir!");
		}
	}
}
