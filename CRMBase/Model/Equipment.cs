using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Независима
	public class Equipment : ValidableModel<Equipment>, INotifyPropertyChanged
	{
		public Equipment()
        {
			Validator = EquipmentValidator.Instance;
        }

		private string name;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}
		private float? price;
		public float? Price
		{
			get => price;
			set
			{
				price = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
			}
		}
		private int? quantity;
		public int? Quantity
		{
			get => quantity;
			set
			{
				quantity = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}