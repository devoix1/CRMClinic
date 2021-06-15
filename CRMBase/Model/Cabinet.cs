using CRMBase.Model.Validators;
using System.Collections.Generic;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Имеет тип связи many-to-many с Medicine, Equipment
	public class Cabinet : ValidableModel<Cabinet>, INotifyPropertyChanged
	{
		public Cabinet()
        {
			Validator = CabinetValidator.Instance;
        }

		private HashSet<Equipment> equipments;
		public HashSet<Equipment> Equipments
		{
			get => equipments;
			set
			{
				equipments = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Equipments)));
			}
		}
		private HashSet<Medicine> medicines;
		public HashSet<Medicine> Medicines
		{
			get => medicines;
			set
			{
				medicines = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Medicines)));
			}
		}
		private int? number;
		public int? Number
		{
			get => number;
			set
			{
				number = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}