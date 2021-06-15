using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class CabinetValidator : AbstractValidator<Cabinet>
	{
		private CabinetValidator()
		{
			RuleFor(c => c.Number)
				.NotNull()
				.NotEmpty()
				.WithMessage("Number must not be empty")
				.GreaterThan(0)
				.WithMessage("Number must be possitive number");
		}




		private static CabinetValidator instance;
		public static CabinetValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new CabinetValidator();
				}
				return instance;
			}
		}

	}


}