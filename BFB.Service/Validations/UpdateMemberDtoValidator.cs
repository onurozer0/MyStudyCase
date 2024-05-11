using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
	{
		public UpdateMemberDtoValidator()
		{
			RuleFor(x => x.Password).NotNull().WithMessage("Parola alanı boş bırakılamaz!");
			RuleFor(x => x.PasswordConfirm).NotNull().WithMessage("Parola Doğrulama alanı boş bırakılamaz!").Must((model, confirmPassword) => confirmPassword == model.Password).WithMessage("Parolalar Eşleşmiyor.");
			RuleFor(x => x.Name).NotNull().WithMessage("Ad alanı boş bırakılamaz!");
			RuleFor(x => x.Surname).NotNull().WithMessage("Soyad alanı boş bırakılamaz!");
			RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Telefon Numarası alanı boş bırakılamaz!");
			RuleFor(x => x.UserAddress.Address).NotNull().WithMessage("Telefon Numarası alanı boş bırakılamaz!");
			RuleFor(x => x.UserAddress.City).NotNull().WithMessage("Telefon Numarası alanı boş bırakılamaz!");
			RuleFor(x => x.UserAddress.Zipcode).NotNull().WithMessage("Posta Kodu Alanı Boş Bırakılamaz!").Length(5).WithMessage("Posta Kodu Geçersizdir!");
		}
	}
}
