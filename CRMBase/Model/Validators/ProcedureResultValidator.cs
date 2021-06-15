using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class ProcedureResultValidator : AbstractValidator<ProcedureResult>
	{
		private ProcedureResultValidator()
		{
			RuleFor(p => p.Recipe)
				.NotNull()
				.NotEmpty()
				.WithMessage("Recipe must not be empty");
			

		}




		private static ProcedureResultValidator instance;
		public static ProcedureResultValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProcedureResultValidator();
				}
				return instance;
			}
		}

	}


}