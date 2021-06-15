using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace CRMAdmin.ViewModel
{
	public sealed class AddPatientVM : SuperViewModel
	{
		public AddPatientVM()
		{
			loginTaskManager.AddTask(() =>
			{
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
				NewPatient = new Patient();
				NewAccount = new Account();
			});
		}
		public List<string> Genders { get; private set; }
		private Patient newPatient;
		public Patient NewPatient
		{
			get => newPatient;
			set => Set(ref newPatient, value);
		}
		private Account newAccount;
		public Account NewAccount
		{
			get => newAccount;
			set => Set(ref newAccount, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			NewAccount.Type = AccountType.Patient;
			await accountTableManager.Insert(NewAccount);
			NewAccount.Id = await procedureManager.ExecuteSingle<int>("LastAccountId");
			NewPatient.Account = NewAccount;
			await patientTableManager.Insert(NewPatient);
			MessengerInstance.Send(new CollectionUpdateMessage<Patient>(this)
			{
				Collection = await patientTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} added new patient {NewAccount.Id}#{NewAccount.Login}"
			});
			NewAccount = new Account();
			NewPatient = new Patient();
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<PatientVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<PatientVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
		}
	}
}
