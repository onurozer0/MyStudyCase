using BFB.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFB.Service.Validations
{
	public class SearchDtoValidator : AbstractValidator<SearchDto>

	{
        public SearchDtoValidator()
        {
            RuleFor(x => x.Word).NotNull().WithMessage("Arama alanı boş bırakılamaz!").MaximumLength(30).WithMessage("En fazla 30 karakterlik arama yapılabilir!");
        }
    }
}
