using CRMAdmin.ViewModel;
using FluentValidation;

namespace CRMAdmin.Validators
{
	public sealed class AccountVMValidator : AbstractValidator<AccountVM>
	{
		public AccountVMValidator()
		{
			RuleFor(a => a.SelectedAdmin)
				.NotNull()
				.DependentRules(() =>
				{
					RuleFor(a => a.SelectedAdmin.Email)
						.NotNull()
						.NotEmpty()
						.WithMessage("Email must not be empty")
						.EmailAddress()
						.WithMessage("Incorrect Email format")
						.MaximumLength(50)
						.WithMessage("Email exceeds the length limit");
					RuleFor(a => a.SelectedAdmin.Name)
						.NotNull()
						.NotEmpty()
						.WithMessage("Name must not be empty")
						.MaximumLength(30)
						.WithMessage("Name exceeds the length limit");
					RuleFor(a => a.SelectedAdmin.Surname)
						.NotNull()
						.NotEmpty()
						.WithMessage("Surname must not be empty")
						.MaximumLength(30)
						.WithMessage("Surname exceeds the length limit");
					RuleFor(a => a.SelectedAdmin.Middlename)
						.NotNull()
						.NotEmpty()
						.WithMessage("Middlename must not be empty")
						.MaximumLength(30)
						.WithMessage("Middlename exceeds the length limit");
					RuleFor(a => a.SelectedAdmin.Gender)
						.NotNull()
						.WithMessage("Gender must be selected");
					RuleFor(a => a.SelectedAdmin.Phone)
						.NotNull()
						.WithMessage("Phone must be filled");
					RuleFor(a => a.SelectedAdmin.BirthDate)
						.NotNull()
						.WithMessage("Date of birth must not be empty");
					RuleFor(a => a.SelectedAdmin.Address)
						.NotNull()
						.WithMessage("Address must not be empty");
				});
		}
	}
}
