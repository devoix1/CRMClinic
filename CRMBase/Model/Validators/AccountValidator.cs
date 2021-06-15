using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Model.Validators
{
	public sealed class AccountValidator : AbstractValidator<Account>
	{
		private AccountValidator()
		{
			RuleFor(a => a.Login)
				.NotNull()
				.NotEmpty()
				.WithMessage("Login must not be empty")
				.MaximumLength(30)
				.WithMessage("Login exceeds the length limit");
			RuleFor(a => a.Password)
				.NotNull()
				.NotEmpty()
				.WithMessage("Password must not be empty")
				.MaximumLength(30)
				.WithMessage("Password exceeds the length limit");

		}
		private static AccountValidator instance;
		public static AccountValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AccountValidator();
				}
				return instance;
			}
		}

	}
}
