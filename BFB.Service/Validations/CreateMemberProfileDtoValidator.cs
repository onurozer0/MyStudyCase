using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class CreateMemberProfileDtoValidator : AbstractValidator<CreateMemberProfileDto>
	{
		public CreateMemberProfileDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().WithMessage("Ad alanı boş bırakılamaz!");
			RuleFor(x => x.Surname).NotNull().WithMessage("Soyad alanı boş bırakılamaz!");
			RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Telefon Numarası alanı boş bırakılamaz!");
			RuleFor(x => x.UserAddress!.Address).NotNull().WithMessage("Adres alanı boş bırakılamaz!");
			RuleFor(x => x.UserAddress!.City).NotNull().WithMessage("Lütfen Şehir Seçiniz!");
			RuleFor(x => x.DateOfBirth).NotNull().WithMessage("Doğum Tarihi Seçimi Yapınız!");
			RuleFor(x => x.UserAddress!.Zipcode).NotNull().WithMessage("Posta Kodu Alanı Boş Bırakılamaz!").Length(5).WithMessage("Posta Kodu Geçersizdir!");
			RuleFor(x => x.IdentityNumber).NotNull().WithMessage("Kimlik Numarası Alanı Boş Bırakılamaz!").Length(11).WithMessage("Kimlik Numarası Geçersizdir!");
		}
	}
}
