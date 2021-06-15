using FluentValidation;

namespace CRMBase.Model.Validators
{
	public sealed class DoctorValidator : PersonValidator<Doctor>
	{
		private DoctorValidator()
		{
			RuleFor(d => d.InterestRate)
				.NotNull()
				.NotEmpty()
				.WithMessage("InterestRate must not be empty")
				.GreaterThan(0)
				.WithMessage("InterestRate must be possitive number");
		}
		private static DoctorValidator instance;
		public static DoctorValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DoctorValidator();
				}
				return instance;
			}
		}
	}
}