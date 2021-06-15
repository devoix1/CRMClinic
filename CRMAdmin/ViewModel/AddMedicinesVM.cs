using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public class AddMedicinesVM : SuperViewModel
	{
		public AddMedicinesVM()
		{
			loginTaskManager.AddTask(() =>
			{
				Medicine = new Medicine();
			});
		}
		private Medicine medicine;
		public Medicine Medicine
		{
			get => medicine;
			set => Set(ref medicine, value);
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<MedicineVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand addMedicineCommand;
		public RelayCommand AddMedicineCommand => addMedicineCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await medicineTableManager.Insert(Medicine);
			MessengerInstance.Send(new CollectionUpdateMessage<Medicine>(this)
			{
				Collection = await medicineTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} added new medicine {Medicine.Name}"
			});
			Medicine = new Medicine();
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<MedicineVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			AddMedicineCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
