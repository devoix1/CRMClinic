using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Зависима от Account
	public class Patient : Person<Patient>
	{
		public Patient()
		{
			Validator = PatientValidator.Instance;
		}
		private Account account;
		public Account Account
		{
			get => account;
			set
			{
				account = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Account)));
			}
		}
	}
}