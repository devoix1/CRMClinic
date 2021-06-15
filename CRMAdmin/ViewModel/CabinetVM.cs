using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace CRMAdmin.ViewModel
{
	public sealed class CabinetVM : SuperViewModel
	{
		public CabinetVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Cabinets = await cabinetTableManager.SelectAll();
			});
			MessengerInstance.Register<ItemUpdateMessage<Equipment>>(this, message =>
			{
				if (message.Sender is EquipmentListVM)
				{
					SelectedEquipment = message.Item;
					SelectedMedicine = null;
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<Medicine>>(this, message =>
			{
				if (message.Sender is MedicineListVM)
				{
					SelectedMedicine = message.Item;
					selectedEquipment = null;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Cabinet>>(this, message =>
			{
				if (message.Sender is AddCabinetVM)
				{
					Cabinets = message.Collection;
				}
			});
			SelectedIndexOfList = -1;
		}
		private SuperViewModel currentVM;
		public SuperViewModel CurrentVM
		{
			get => currentVM;
			set => Set(ref currentVM, value);
		}
		private RelayCommand createCommand;
		public RelayCommand CreateCommand => createCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddCabinetVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private Equipment selectedEquipment;
		public Equipment SelectedEquipment
		{
			get => selectedEquipment;
			set => Set(ref selectedEquipment, value);
		}
		private Medicine selectedMedicine;
		public Medicine SelectedMedicine
		{
			get => selectedMedicine;
			set => Set(ref selectedMedicine, value);
		}
		private string addButtonText;
		public string AddButtonText
		{
			get => addButtonText;
			set => Set(ref addButtonText, value);
		}
		private int selectedIndexOfList;
		public int SelectedIndexOfList
		{
			get => selectedIndexOfList;
			set
			{
				Set(ref selectedIndexOfList, value);
				if (SelectedIndexOfList == 0)
				{
					Task.Run(async () =>
					{
						MessengerInstance.Send(new CollectionUpdateMessage<Medicine>(this)
						{
							Collection = await medicineTableManager.SelectAll()
						});
					});
					AddButtonText = "Add Medicine";
					CurrentVM = App.Container.GetInstance<MedicineListVM>();
				}
				if (SelectedIndexOfList == 1)
				{
					Task.Run(async () =>
					{
						MessengerInstance.Send(new CollectionUpdateMessage<Equipment>(this)
						{
							Collection = await equipmentTableManager.SelectAll()
						});
					});
					AddButtonText = "Add Equipment";
					CurrentVM = App.Container.GetInstance<EquipmentListVM>();
				}
				TriggerCommandExecution();
			}
		}
		private Cabinet selectedCabinet;
		public Cabinet SelectedCabinet
		{
			get => selectedCabinet;
			set => Set(ref selectedCabinet, value);
		}
		private HashSet<Cabinet> cabinets;
		public HashSet<Cabinet> Cabinets
		{
			get => cabinets;
			set => Set(ref cabinets, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			if (SelectedCabinet == null)
			{
				return;
			}
			var cabinet = SelectedCabinet;
			if (SelectedMedicine != null)
			{
				var medicine = SelectedMedicine;
				if (medicine.Quantity == 0)
				{
					MessageBox.Show("Not enough quantity");
					return;
				}
				LockCommandExecution();
				await Task.Delay(500);
				var @params = new DynamicParameters();
				@params.Add("id_cabinet", cabinet.Id);
				@params.Add("id_medicine", medicine.Id);
				await procedureManager.ExecuteVoid("InsertCabinetMedicine", @params);
				medicine.Quantity -= 1;
				await medicineTableManager.UpdateById(medicine.Id, medicine);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} added medicine {SelectedMedicine.Name} to {SelectedCabinet.Number} cabinet"
				});
				MessengerInstance.Send(new CollectionUpdateMessage<Medicine>(this)
				{
					Collection = await medicineTableManager.SelectAll()
				});
				UnlockCommandExecution();
				return;
			}
			if (SelectedEquipment != null)
			{
				var equipment = selectedEquipment;
				if (equipment.Quantity == 0)
				{
					MessageBox.Show("Not enough quantity");
					return;
				}
				LockCommandExecution();
				var @params = new DynamicParameters();
				@params.Add("id_cabinet", cabinet.Id);
				@params.Add("id_equipment", equipment.Id);
				await procedureManager.ExecuteVoid("InsertCabinetEquipment", @params);
				equipment.Quantity -= 1;
				await equipmentTableManager.UpdateById(equipment.Id, equipment);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} added equipment {SelectedEquipment.Name} to {SelectedCabinet.Number} cabinet"
				});
				MessengerInstance.Send(new CollectionUpdateMessage<Equipment>(this)
				{
					Collection = await equipmentTableManager.SelectAll()
				});
				UnlockCommandExecution();
				return;
			}
		}, () => !ExecutionLocked && SelectedIndexOfList != -1);
		private RelayCommand showInfoCommand;
		public RelayCommand ShowInfoCommand => showInfoCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<ShowCabinetInfoVM>()
			}, DashboardVMToken.Instance);
		});
		protected override void TriggerCommandExecution()
		{
			AddCommand.RaiseCanExecuteChanged();
			CreateCommand.RaiseCanExecuteChanged();
		}
	}
}
