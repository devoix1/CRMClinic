using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.TableManagers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CRMAdmin.ViewModel
{
	public abstract class SuperViewModel : ViewModelBase
	{
		public SuperViewModel() : base(App.Container.GetInstance<Messenger>())
		{
			MessengerInstance.Register<ItemUpdateMessage<Account>>(this, message =>
			{
				if (message.Sender is AccountVM || message.Sender is DoctorVM)
				{
					return;
				}
				currentAccount = message.Item;
			});
		}
		protected readonly ITableManager<Account> accountTableManager = App.Container.GetInstance<AccountTableManager>();
		protected readonly ITableManager<Admin> adminTableManager = App.Container.GetInstance<AdminTableManager>();
		protected readonly ITableManager<Cabinet> cabinetTableManager = App.Container.GetInstance<CabinetTableManager>();
		protected readonly ITableManager<Category> categoryTableManager = App.Container.GetInstance<CategoryTableManager>();
		protected readonly ITableManager<Doctor> doctorTableManager = App.Container.GetInstance<DoctorTableManager>();
		protected readonly ITableManager<Equipment> equipmentTableManager = App.Container.GetInstance<EquipmentTableManager>();
		protected readonly ITableManager<Log> logTableManager = App.Container.GetInstance<LogTableManager>();
		protected readonly ITableManager<Medicine> medicineTableManager = App.Container.GetInstance<MedicineTableManager>();
		protected readonly ITableManager<Patient> patientTableManager = App.Container.GetInstance<PatientTableManager>();
		protected readonly ITableManager<Procedure> procedureTableManager = App.Container.GetInstance<ProcedureTableManager>();
		protected readonly ITableManager<Record> recordTableManager = App.Container.GetInstance<RecordTableManager>();
		protected readonly IProcedureManager procedureManager = new ProcedureManager();
		protected readonly IAppTaskManager loginTaskManager = App.Container.GetInstance<AppTaskManager>();
		protected Account currentAccount;
		protected virtual bool ExecutionLocked { get; set; } = false;
		protected void LockCommandExecution()
		{
			ExecutionLocked = true;
			TriggerCommandExecution();
		}
		protected void UnlockCommandExecution()
		{
			ExecutionLocked = false;
			TriggerCommandExecution();
		}
		protected abstract void TriggerCommandExecution();
	}
}