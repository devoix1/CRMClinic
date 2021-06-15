using FluentValidation;

namespace CRMBase.Model.Validators
{
	public class PersonValidator<T> : AbstractValidator<T>
		where T : Person<T>
	{
		protected PersonValidator()
		{
			RuleFor(p => p.Email)
				.NotNull()
				.NotEmpty()
				.WithMessage("Email must not be empty")
				.EmailAddress()
				.WithMessage("Incorrect Email format")
				.MaximumLength(50)
				.WithMessage("Email exceeds the length limit");
			RuleFor(p => p.Name)
				.NotNull()
				.NotEmpty()
				.WithMessage("Name must not be empty")
				.MaximumLength(30)
				.WithMessage("Name exceeds the length limit");
			RuleFor(p => p.Surname)
				.NotNull()
				.NotEmpty()
				.WithMessage("Surname must not be empty")
				.MaximumLength(30)
				.WithMessage("Surname exceeds the length limit");
			RuleFor(p => p.Middlename)
				.NotNull()
				.NotEmpty()
				.WithMessage("Middlename must not be empty")
				.MaximumLength(30)
				.WithMessage("Middlename exceeds the length limit");
			RuleFor(p => p.Gender)
				.NotNull()
				.WithMessage("Gender must be selected");
			RuleFor(p => p.Phone)
				.NotNull()
				.NotEmpty()
				.WithMessage("Phone must not be empty")
				.Matches(@"\d{3}-\d{3}-\d{2}-\d{2}")
				.WithMessage("Incorrect phone format . Use 0XX-XXX-XX-XX");
			RuleFor(p => p.Address)
				.NotNull()
				.NotEmpty()
				.WithMessage("Address must not be empty");
			RuleFor(p => p.BirthDate)
				.NotNull()
				.NotEmpty()
				.WithMessage("BirthDate must not be empty");
		}
	}
}
