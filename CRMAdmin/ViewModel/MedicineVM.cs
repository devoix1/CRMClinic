using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMAdmin.ViewModel
{
	public sealed class MedicineVM : SuperViewModel
	{
		public MedicineVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Medicines = await medicineTableManager.SelectAll();
			});
			MessengerInstance.Register<CollectionUpdateMessage<Medicine>>(this, message =>
			{
				if (message.Sender is AddMedicinesVM)
				{
					Medicines = message.Collection;
				}
			});
		}
		private Medicine selectedMedicine;
		public Medicine SelectedMedicine
		{
			get => selectedMedicine;
			set => Set(ref selectedMedicine, value);
		}
		private HashSet<Medicine> medicines;
		public HashSet<Medicine> Medicines
		{
			get => medicines;
			set => Set(ref medicines, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddMedicinesVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand removeCommand;
		public RelayCommand RemoveCommand => removeCommand ??= new RelayCommand(async () =>
		{
			if (SelectedMedicine != null)
			{
				LockCommandExecution();
				await medicineTableManager.DeleteById(SelectedMedicine.Id);
				var medicines = await medicineTableManager.SelectAll();
				Medicines = medicines;
				MessengerInstance.Send(new CollectionUpdateMessage<Medicine>(this)
				{
					Collection = medicines
				});
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Delete,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} removed {SelectedMedicine.Name} medicine"
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked);
		private RelayCommand modifyCommand;
		public RelayCommand ModifyCommand => modifyCommand ??= new RelayCommand(async () =>
		{
			if (SelectedMedicine != null)
			{
				LockCommandExecution();
				await medicineTableManager.UpdateById(SelectedMedicine.Id, SelectedMedicine);
				var medicines = await medicineTableManager.SelectAll();
				Medicines = medicines;
				MessengerInstance.Send(new CollectionUpdateMessage<Medicine>(this)
				{
					Collection = medicines
				});
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} modified {SelectedMedicine.Name} medicine"
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			ModifyCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			RemoveCommand.RaiseCanExecuteChanged();
		}
	}
}
