using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System;

namespace CRMAdmin.ViewModel
{
	public sealed class SetDoctorRecordsAnalyseVM : SuperViewModel
	{
		public SetDoctorRecordsAnalyseVM()
		{
			MessengerInstance.Register<ItemUpdateMessage<Analysis>>(this, message =>
			{
				if (message.Sender is DoctorRecordsVM)
				{
					if (message.Item == null)
					{
						Analysis = new Analysis();
						isNew = true;
					}
					else
					{
						Analysis = message.Item;
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
		private bool isNew = false;
		private ProcedureResult procedureResult;
		private Analysis analysis;
		public Analysis Analysis
		{
			get => analysis;
			set => Set(ref analysis, value);
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			Analysis = null;
			procedureResult = null;
			isNew = false;
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var @params = new DynamicParameters();
			@params.Add("name", Analysis.Name);
			@params.Add("result", Analysis.Result);
			@params.Add("time_stamp", DateTime.Now);
			@params.Add("is_done", Analysis.IsDone);
			@params.Add("id_analysis_procedure_result", procedureResult.Id);
			if (isNew)
			{
				await procedureManager.ExecuteVoid("InsertAnalysis", @params);
			}
			else
			{
				@params.Add("id", Analysis.Id);
				await procedureManager.ExecuteVoid("UpdateAnalysisById", @params);
			}
			UnlockCommandExecution();
			if (BackCommand.CanExecute(null))
			{
				BackCommand.Execute(null);
			}
		}, () => !ExecutionLocked);
		private RelayCommand changeIsDone;
		public RelayCommand ChangeIsDone => changeIsDone ??= new RelayCommand(() =>
		{
			Analysis.IsDone = !Analysis.IsDone;
		});
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
