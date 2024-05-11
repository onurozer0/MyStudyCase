using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
	{
		public ResetPasswordDtoValidator()
		{
			RuleFor(x => x.NewPassword).NotNull().WithMessage("Şifre Alanı Boş Bırakılamaz.");
			RuleFor(x => x.ConfirmPassword).NotNull().WithMessage("Şifre Doğrulama Alanı Boş Bırakılamaz.").Must((model, confirmPassword) => confirmPassword == model.NewPassword)
			.WithMessage("Şifreler Eşleşmiyor.");
		}
	}
}
