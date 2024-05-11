using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class SignInDtoValidator : AbstractValidator<SignInDto>
	{
		public SignInDtoValidator()
		{
			RuleFor(x => x.Email).NotNull().WithMessage("Lütfen geçerli bir e-posta adresi girin.").EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin.");
			RuleFor(x => x.Password).NotNull().WithMessage("Lütfen geçerli bir parola belirleyin.");
		}
	}
}
