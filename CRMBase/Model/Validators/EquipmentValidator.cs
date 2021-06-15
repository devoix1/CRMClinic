using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class EquipmentValidator : AbstractValidator<Equipment>
	{
		private EquipmentValidator()
		{
			RuleFor(e => e.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("Name must not be empty")
				.MaximumLength(30)
				.WithMessage("Name exceeds the length limit");
			RuleFor(e=>e.Price)
				.NotNull()
				.NotEmpty()
				.WithMessage("Price must not be empty")
				.GreaterThan(0)
				.WithMessage("Price must be possitive number");
			RuleFor(e => e.Quantity)
				.NotNull()
				.NotEmpty()
				.WithMessage("Quantity must not be empty")
				.GreaterThan(0)
				.WithMessage("Quantity must be possitive number");

		}




		private static EquipmentValidator instance;
		public static EquipmentValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EquipmentValidator();
				}
				return instance;
			}
		}

	}


}