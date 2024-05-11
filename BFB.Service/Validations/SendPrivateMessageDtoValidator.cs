using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	internal class SendPrivateMessageDtoValidator : AbstractValidator<SendPrivateMessageDto>
	{
		public SendPrivateMessageDtoValidator()
		{
			RuleFor(x => x.Message).NotNull().WithMessage("Mesaj alanı boş bırakılamaz!").MaximumLength(2000).WithMessage("Maksimum 2000 karakter uzunluğunda mesaj gönderilebilir!");
		}
	}
}
