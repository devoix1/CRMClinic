using CRMBase.Model.Validators;
using System;
using System.ComponentModel;

namespace CRMBase.Model
{
	public class Analysis : ValidableModel<Analysis>, INotifyPropertyChanged
	{

		public Analysis()
        {
			Validator = AnalysisValidator.Instance;
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
		private string result;
		public string Result
		{
			get => result;
			set
			{
				result = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
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
		private bool? isDone;
		public bool? IsDone
		{
			get => isDone;
			set
			{
				isDone = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDone)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}