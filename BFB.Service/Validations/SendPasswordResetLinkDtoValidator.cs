using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class SendPasswordResetLinkDtoValidator : AbstractValidator<SendPasswordResetLinkDto>
	{
		public SendPasswordResetLinkDtoValidator()
		{
			RuleFor(x => x.Email).NotNull().WithMessage("E-Posta alanı boş bırakılamaz!").EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin!");
		}
	}
}
