using CRMBase.Model.Validators;
using System;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Зависима от Doctor
	public class Diagnosis : ValidableModel<Diagnosis>, INotifyPropertyChanged
	{

		public Diagnosis()
        {
			Validator = DiagnosisValidator.Instance;
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
		private DateTime? timestamp;
		public DateTime? Timestamp
		{
			get => timestamp;
			set
			{
				timestamp = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Timestamp)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}