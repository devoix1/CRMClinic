using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Независима
	public class Category : ValidableModel<Category>, INotifyPropertyChanged
	{
		public Category()
        {
			Validator = CategoryValidator.Instance;
        }

		private string value;
		public string Value
		{
			get => value;
			set
			{
				this.value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}