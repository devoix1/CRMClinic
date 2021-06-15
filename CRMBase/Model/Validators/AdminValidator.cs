namespace CRMBase.Model.Validators
{
	public sealed class AdminValidator : PersonValidator<Admin>
	{
		private AdminValidator()
		{

		}
		private static AdminValidator instance;
		public static AdminValidator Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AdminValidator();
				}
				return instance;
			}
		}
	}
}