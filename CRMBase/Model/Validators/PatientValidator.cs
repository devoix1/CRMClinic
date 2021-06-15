namespace CRMBase.Model.Validators
{
	public sealed class PatientValidator : PersonValidator<Patient>
	{
		private static PatientValidator instance;
		public static PatientValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PatientValidator();
				}
				return instance;
			}
		}
	}
}