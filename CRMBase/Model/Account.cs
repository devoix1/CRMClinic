using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Независима
	public class Account : ValidableModel<Account>, INotifyPropertyChanged 
	{
		public Account ()
        {
			Validator = AccountValidator.Instance;
        }

		private AccountType? type;
		public AccountType? Type
		{
			get => type;
			set
			{
				type = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
			}
		}
		private string login;
		public string Login
		{
			get => login;
			set
			{
				login = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
			}
		}
		private string password;
		public string Password
		{
			get => password;
			set
			{
				password = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}