using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class MedicineValidator : AbstractValidator<Medicine>
	{
		private MedicineValidator()
		{
			RuleFor(m => m.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("Name must not be empty");
			RuleFor(m => m.Price)
				.NotNull()
				.NotEmpty()
				.WithMessage("Price must not be empty")
			    .GreaterThan(0)
				.WithMessage("Price must be possitive number");
			RuleFor(m => m.Quantity)
				.NotNull()
				.NotEmpty()
				.WithMessage("Quantity must not be empty")
				.GreaterThan(0)
				.WithMessage("Quantity must be possitive number");
			RuleFor(m => m.Doses)
				.NotNull()
				.NotEmpty()
				.WithMessage("Doses must not be empty")
				.GreaterThan(0)
				.WithMessage("Doses must be possitive number");

		}




		private static MedicineValidator instance;
		public static MedicineValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MedicineValidator();
				}
				return instance;
			}
		}

	}


}