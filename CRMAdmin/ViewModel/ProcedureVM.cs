using CRMBase.Messages;
using CRMBase.Model;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class ProcedureVM : SuperViewModel
	{
		private Procedure procedure = new Procedure();
		public Procedure Procedure
		{
			get => procedure;
			set => Set(ref procedure, value);
		}
		private RelayCommand addProcedureCommand;
		public RelayCommand AddProcedureCommand => addProcedureCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await procedureTableManager.Insert(Procedure);
			MessengerInstance.Send(new CollectionUpdateMessage<Procedure>(this)
			{
				Collection = await procedureTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} added new procedure {Procedure.Name}"
			});
			UnlockCommandExecution();
			Procedure = new Procedure();
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			AddProcedureCommand.RaiseCanExecuteChanged();
		}
	}
}
