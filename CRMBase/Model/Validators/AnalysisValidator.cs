using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class AnalysisValidator : AbstractValidator<Analysis>
	{
		private AnalysisValidator()
		{
			RuleFor(a => a.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("Name must not be empty")
				.MaximumLength(30)
				.WithMessage("Name exceeds the length limit");
			RuleFor(a => a.Result)
				.NotNull()
				.NotEmpty()
				.WithMessage("Result must not be empty");
			RuleFor(a=> a.Timestamp)
				.NotNull()
				.NotEmpty()
				.WithMessage("Timestamp must not be empty");

		}




		private static AnalysisValidator instance;
		public static AnalysisValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AnalysisValidator();
				}
				return instance;
			}
		}

	}


}
