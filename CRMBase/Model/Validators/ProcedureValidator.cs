using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class ProcedureValidator : AbstractValidator<Procedure>
	{
		private ProcedureValidator()
		{
			RuleFor(p => p.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("Name must not be empty");
			RuleFor(p => p.Price)
				.NotNull()
				.NotEmpty()
				.WithMessage("Price must not be empty")
				.GreaterThan(0)
			    .WithMessage("Price must be possitive number");
			RuleFor(p => p.Description)
				.NotNull()
				.NotEmpty()
				.WithMessage("Description must not be empty");
			RuleFor(p=> p.Duration)
				.NotNull()
				.NotEmpty()
				.WithMessage("Duration must not be empty")
			    .GreaterThan(0)
				.WithMessage("Duration must be possitive number");

		}




		private static ProcedureValidator instance;
		public static ProcedureValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProcedureValidator();
				}
				return instance;
			}
		}

	}


}