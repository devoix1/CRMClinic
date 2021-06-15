using CRMBase.Model.Validators;
using System;
using System.ComponentModel;

namespace CRMBase.Model
{
	public class ProcedureResult : ValidableModel<ProcedureResult>, INotifyPropertyChanged
	{
		public ProcedureResult()
		{
			Validator = ProcedureResultValidator.Instance;
		}
		private string recipe;
		public string Recipe
		{
			get => recipe;
			set
			{
				recipe = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Recipe)));
			}
		}
		private DateTime? beginTimestamp;
		public DateTime? ProcudureTimestampBegin
		{
			get => beginTimestamp;
			set
			{
				beginTimestamp = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcudureTimestampBegin)));
			}
		}
		private DateTime? endTimestamp;
		public DateTime? ProcudureTimestampEnd
		{
			get => endTimestamp;
			set
			{
				endTimestamp = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcudureTimestampEnd)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}