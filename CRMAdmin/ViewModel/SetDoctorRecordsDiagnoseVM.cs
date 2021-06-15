using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System;

namespace CRMAdmin.ViewModel
{
	public sealed class SetDoctorRecordsDiagnoseVM : SuperViewModel
	{
		public SetDoctorRecordsDiagnoseVM()
		{
			MessengerInstance.Register<ItemUpdateMessage<Diagnosis>>(this, message =>
			{
				if (message.Sender is DoctorRecordsVM)
				{
					if (message.Item == null)
					{
						Diagnosis = new Diagnosis();
						isNew = true;
					}
					else
					{
						Diagnosis = message.Item;
					}
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<ProcedureResult>>(this, message =>
			{
				if (message.Sender is DoctorRecordsVM)
				{
					procedureResult = message.Item;
				}
			});
		}
		private ProcedureResult procedureResult;
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			Diagnosis = null;
			procedureResult = null;
			isNew = false;
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private Diagnosis diagnosis;
		public Diagnosis Diagnosis
		{
			get => diagnosis;
			set => Set(ref diagnosis, value);
		}
		private bool isNew = false;
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var @params = new DynamicParameters();
			@params.Add("description", Diagnosis.Description);
			@params.Add("time_stamp", DateTime.Now);
			@params.Add("id_diagnosis_procedure_result", procedureResult.Id);
			if (!isNew)
			{
				@params.Add("id", Diagnosis.Id);
				await procedureManager.ExecuteVoid("UpdateDiagnosisById", @params);
			}
			else
			{
				await procedureManager.ExecuteVoid("InsertDiagnosis", @params);
			}
			UnlockCommandExecution();
			if (BackCommand.CanExecute(null))
			{
				BackCommand.Execute(null);
			}
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}