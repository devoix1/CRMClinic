using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class CategoryValidator : AbstractValidator<Category>
	{
		private CategoryValidator()
		{
			RuleFor(c => c.Value)
				.NotNull()
				.NotEmpty()
				.WithMessage("Value must not be empty");
		}


		private static CategoryValidator instance;
		public static CategoryValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new CategoryValidator();
				}
				return instance;
			}
		}

	}


}