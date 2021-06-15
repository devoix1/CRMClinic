using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class AddEquipmentVM : SuperViewModel
	{
		public AddEquipmentVM()
		{
			loginTaskManager.AddTask(() =>
			{
				Equipment = new Equipment();
			});
		}
		private Equipment equipment;
		public Equipment Equipment
		{
			get => equipment;
			set => Set(ref equipment, value);
		}
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await equipmentTableManager.Insert(Equipment);
			MessengerInstance.Send(new CollectionUpdateMessage<Equipment>(this)
			{
				Collection = await equipmentTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} added new equipment {Equipment.Name}"
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<EquipmentVM>()
			}, DashboardVMToken.Instance);
			Equipment = new Equipment();
		}, () => !ExecutionLocked);
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<EquipmentVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
