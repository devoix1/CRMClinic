using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class DiagnosisValidator : AbstractValidator<Diagnosis>
	{
		private DiagnosisValidator()
		{
			RuleFor(d => d.Description)
				.NotNull()
				.NotEmpty()
				.WithMessage("Description must not be empty");
			RuleFor(d => d.Timestamp)
				.NotNull()
				.NotEmpty()
				.WithMessage("Timestamp must not be empty");
		}


		private static DiagnosisValidator instance;
		public static DiagnosisValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DiagnosisValidator();
				}
				return instance;
			}
		}

	}


}