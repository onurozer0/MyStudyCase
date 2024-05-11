using BFB.Core.DTOs;
using FluentValidation;

namespace BFB.Service.Validations
{
	public class SendOfferDtoValidator : AbstractValidator<SendOfferDto>
	{
		public SendOfferDtoValidator()
		{
			RuleFor(x => x.Price).NotNull().WithMessage("Fiyat teklifi alanı boş bırakılamaz!");
			RuleFor(x => x.Title).NotNull().WithMessage("Başlık alanı boş bırakılamaz!").MaximumLength(150).WithMessage("Maksimum 150 karakter başlık girilebilir!");
			RuleFor(x => x.Message).NotNull().WithMessage("Mesaj alanı boş bırakılamaz!").MaximumLength(2000).WithMessage("Maksimum 2000 karakter mesaj girilebilir!");
		}
	}
}
