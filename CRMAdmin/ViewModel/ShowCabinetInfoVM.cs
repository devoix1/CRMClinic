using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMAdmin.ViewModel
{
	public sealed class ShowCabinetInfoVM : SuperViewModel
	{
		public ShowCabinetInfoVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Cabinets = await cabinetTableManager.SelectAll();
			});
			loginTaskManager.AddTask(() =>
			{
				Cabinets = null;
				Equipment = null;
				Medicines = null;
				Cabinets = null;
			});
			MessengerInstance.Register<CollectionUpdateMessage<Cabinet>>(this, message =>
			{
				if (message.Sender is AddCabinetVM)
				{
					Cabinets = message.Collection;
				}
			});
		}
		private HashSet<Equipment> equipment;
		public HashSet<Equipment> Equipment
		{
			get => equipment;
			set => Set(ref equipment, value);
		}
		private HashSet<Medicine> medicines;
		public HashSet<Medicine> Medicines
		{
			get => medicines;
			set => Set(ref medicines, value);
		}
		private HashSet<Cabinet> cabinets;
		public HashSet<Cabinet> Cabinets
		{
			get => cabinets;
			set => Set(ref cabinets, value);
		}
		private Cabinet selectedCabinet;
		public Cabinet SelectedCabinet
		{
			get => selectedCabinet;
			set
			{
				Set(ref selectedCabinet, value);
				if (value != null)
				{
					LockCommandExecution();
					Task.WaitAll(Task.Run(async () =>
					{
						var @params = new DynamicParameters();
						@params.Add("cabinet_id", SelectedCabinet.Id);
						Medicines = await procedureManager.Execute<Medicine>("SelectMedicineByCabinetId", @params);
					}), Task.Run(async () =>
					{
						var @params = new DynamicParameters();
						@params.Add("cabinet_id", SelectedCabinet.Id);
						Equipment = await procedureManager.Execute<Equipment>("SelectEquipmentByCabinetId", @params);
					}));
					UnlockCommandExecution();
				}
			}
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<CabinetVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
