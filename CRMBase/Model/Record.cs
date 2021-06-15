using System;
using System.ComponentModel;

namespace CRMBase.Model
{
	// Зависима от Doctor, Patient, Procedure, ProcedureResult
	public class Record : HashComparator, INotifyPropertyChanged
	{
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
		private Doctor doctor;
		public Doctor Doctor
		{
			get => doctor;
			set
			{
				doctor = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Doctor)));
			}
		}
		private Patient patient;
		public Patient Patient
		{
			get => patient;
			set
			{
				patient = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Patient)));
			}
		}
		private Procedure procedure;
		public Procedure Procedure
		{
			get => procedure;
			set
			{
				procedure = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Procedure)));
			}
		}
		private ProcedureResult procedureResult;
		public ProcedureResult ProcedureResult
		{
			get => procedureResult;
			set
			{
				procedureResult = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcedureResult)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}