using CRMBase.Messages;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using CRMBase.Model;
using CRMBase.Tokens;
using System.Threading.Tasks;
using CRMBase.Services;
using Dapper;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public sealed class AddDoctorVM : SuperViewModel
	{
		public AddDoctorVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Categories = await categoryTableManager.SelectAll();
				Cabinets = await cabinetTableManager.SelectAll();
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
				NewDoctor = new Doctor();
				NewAccount = new Account();
			});
			MessengerInstance.Register<ItemUpdateMessage<Procedure>>(this, message =>
			{
				if (message.Sender is SetDoctorProcedureVM)
				{
					selectedProcedures.Add(message.Item);
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Category>>(this, message =>
			{
				if (message.Sender is AddDoctorCategoryVM)
				{
					Categories = message.Collection;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Cabinet>>(this, message =>
			{
				Cabinets = message.Collection;
			});
		}
		public List<string> Genders { get; private set; }
		private Doctor newDoctor;
		public Doctor NewDoctor
		{
			get => newDoctor;
			set => Set(ref newDoctor, value);
		}
		private Account newAccount;
		public Account NewAccount
		{
			get => newAccount;
			set => Set(ref newAccount, value);
		}
		private HashSet<Cabinet> cabinets;
		public HashSet<Cabinet> Cabinets
		{
			get => cabinets;
			set => Set(ref cabinets, value);
		}
		private HashSet<Category> categories;
		public HashSet<Category> Categories
		{
			get => categories;
			set => Set(ref categories, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			if (selectedProcedures.Count == 0)
			{
				return;
			}
			LockCommandExecution();
			NewAccount.Type = AccountType.Doctor;
			await accountTableManager.Insert(NewAccount);
			var accountId = await procedureManager.ExecuteSingle<int>("LastAccountId");
			NewAccount.Id = accountId;
			NewDoctor.Account = NewAccount;
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} created new doctor {accountId}#{NewAccount.Login}"
			});
			await doctorTableManager.Insert(NewDoctor);
			var doctorId = await procedureManager.ExecuteSingle<int>("LastDoctorId");
			var tasks = new HashSet<Task>();
			foreach (var procedure in selectedProcedures)
			{
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("id_doctor", doctorId);
					@params.Add("id_procedure", procedure.Id);
					await procedureManager.ExecuteVoid("InsertDoctorProcedures", @params);
				}));
			}
			Task.WaitAll(tasks.ToArray());
			MessengerInstance.Send(new CollectionUpdateMessage<Doctor>(this)
			{
				Collection = await doctorTableManager.SelectAll()
			});
			NewAccount = new Account();
			NewDoctor = new Doctor();
			selectedProcedures.Clear();
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			NewAccount = new Account();
			NewDoctor = new Doctor();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private HashSet<Procedure> selectedProcedures = new HashSet<Procedure>();
		private RelayCommand setProcedureCommand;
		public RelayCommand SetProcedureCommand => setProcedureCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			selectedProcedures.Clear();
			MessengerInstance.Send(new CollectionUpdateMessage<Procedure>(this)
			{
				Collection = await procedureTableManager.SelectAll()
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<SetDoctorProcedureVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			SetProcedureCommand.RaiseCanExecuteChanged();
		}
	}
}
