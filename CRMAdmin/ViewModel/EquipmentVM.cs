using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace CRMAdmin.ViewModel
{
	public sealed class EquipmentVM : SuperViewModel
	{

		public EquipmentVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Equipments = await equipmentTableManager.SelectAll();
			});
			MessengerInstance.Register<CollectionUpdateMessage<Equipment>>(this, message =>
			{
				if (message.Sender is AddEquipmentVM)
				{
					Equipments = message.Collection;
				}
			});
		}
		private HashSet<Equipment> equipments;
		public HashSet<Equipment> Equipments
		{
			get => equipments;
			set => Set(ref equipments, value);
		}
		private Equipment selectedEquipment;
		public Equipment SelectedEquipment
		{
			get => selectedEquipment;
			set => Set(ref selectedEquipment, value);
		}
		private RelayCommand addEquipmentCommand;
		public RelayCommand AddEquipmentCommand => addEquipmentCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddEquipmentVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand modifyEquipmentCommand;
		public RelayCommand ModifyEquipmentCommand => modifyEquipmentCommand ??= new RelayCommand(async () =>
		{
			if (SelectedEquipment != null)
			{
				LockCommandExecution();
				await equipmentTableManager.UpdateById(SelectedEquipment.Id, SelectedEquipment);
				var equipment = await equipmentTableManager.SelectAll();
				Equipments = equipment;
				MessengerInstance.Send(new CollectionUpdateMessage<Equipment>(this)
				{
					Collection = equipment
				});
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} modified {SelectedEquipment.Name} equipment"
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked);
		private RelayCommand removeEquipmentCommand;
		public RelayCommand RemoveEquipmentCommand => removeEquipmentCommand ??= new RelayCommand(async () =>
		{
			if (SelectedEquipment != null)
			{
				LockCommandExecution();
				await equipmentTableManager.DeleteById(SelectedEquipment.Id);
				var equipment = await equipmentTableManager.SelectAll();
				Equipments = equipment;
				MessengerInstance.Send(new CollectionUpdateMessage<Equipment>(this)
				{
					Collection = equipment
				});
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Delete,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} removed {SelectedEquipment.Name} equipment"
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			ModifyEquipmentCommand.RaiseCanExecuteChanged();
			AddEquipmentCommand.RaiseCanExecuteChanged();
			RemoveEquipmentCommand.RaiseCanExecuteChanged();
		}
	}
}
