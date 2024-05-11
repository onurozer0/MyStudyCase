using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class UserAddressesDtoValidator : AbstractValidator<UserAddressesDto>
	{
		public UserAddressesDtoValidator()
		{

			RuleFor(x => x.Address).NotNull().WithMessage("Adres bilgisi alanı boş bırakılamaz!");
			RuleFor(x => x.City).NotNull().WithMessage("Şehir seçimi yapınız!");
			RuleFor(x => x.Zipcode).NotNull().WithMessage("Posta Kodu Alanı Boş Bırakılamaz!").Length(5).WithMessage("Posta Kodu Geçersizdir!");
		}
	}
}
