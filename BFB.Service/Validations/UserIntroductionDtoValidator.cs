using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class UserIntroductionDtoValidator : AbstractValidator<UserIntroductionDto>
	{
		public UserIntroductionDtoValidator()
		{
			RuleFor(x => x.Message).NotNull().WithMessage("Lütfen geçerli bir mesaj giriniz.");
		}
	}
}
