using CRMBase.Model.Validators;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Независима
	public class Procedure : ValidableModel<Procedure>, INotifyPropertyChanged
	{

		public Procedure()
        {
			Validator = ProcedureValidator.Instance;
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
		private string description;
		public string Description
		{
			get => description;
			set
			{
				description = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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
		private int? duration;
		public int? Duration
		{
			get => duration;
			set
			{
				duration = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}