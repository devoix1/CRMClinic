using CRMAdmin.ViewModel;
using FluentValidation;

namespace CRMAdmin.Validators
{
	public sealed class LoginVMValidator : AbstractValidator<LoginVM>
	{
		public LoginVMValidator()
		{
			RuleFor(p => p.Login)
				.NotNull()
				.NotEmpty()
				.WithMessage("Login must not be empty");
			RuleFor(p => p.MimicPassword)
				.NotNull()
				.NotEmpty()
				.WithMessage("Password must not be empty");
		}
	}
}
