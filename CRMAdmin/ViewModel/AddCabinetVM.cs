using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class AddCabinetVM : SuperViewModel
	{
		public AddCabinetVM()
		{
			loginTaskManager.AddTask(() =>
			{
				Cabinet = new Cabinet();
			});
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<CabinetVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private Cabinet cabinet;
		public Cabinet Cabinet
		{
			get => cabinet;
			set => Set(ref cabinet, value);
		}
		private RelayCommand createCabinetCommand;
		public RelayCommand CreateCabinetCommand => createCabinetCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await cabinetTableManager.Insert(Cabinet);
			MessengerInstance.Send(new CollectionUpdateMessage<Cabinet>(this)
			{
				Collection = await cabinetTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} created new cabinet {Cabinet.Number}"
			});
			Cabinet = new Cabinet();
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<CabinetVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			CreateCabinetCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
