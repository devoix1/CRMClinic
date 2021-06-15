using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Зависима от Account
	public class Admin : Person<Admin>
	{
		public Admin()
		{
			Validator = AdminValidator.Instance;
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
		private bool? isMain;
		public bool? IsMain
		{
			get => isMain;
			set
			{
				isMain = value;
				RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(IsMain)));
			}
		}
	}
}
