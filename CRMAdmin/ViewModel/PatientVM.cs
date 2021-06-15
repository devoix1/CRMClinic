using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public sealed class PatientVM : SuperViewModel
	{
		public PatientVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Patients = originalPatients = await patientTableManager.SelectAll();
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Patient>>(this, message =>
			{
				if (message.Sender is AddPatientVM)
				{
					Patients = originalPatients = message.Collection;
					SearchText = null;
				}
			});
		}
		private readonly ISearchService<Patient, string> patientSearchService = new PatientSearchService();
		private string searchText;
		public string SearchText
		{
			get => searchText;
			set => Set(ref searchText, value);
		}
		public List<string> Genders { get; private set; }
		private Patient selectedPatient;
		public Patient SelectedPatient
		{
			get => selectedPatient;
			set
			{
				Set(ref selectedPatient, value);
				TriggerCommandExecution();
			}
		}
		private HashSet<Patient> originalPatients;
		private HashSet<Patient> patients;
		public HashSet<Patient> Patients
		{
			get => patients;
			set => Set(ref patients, value);
		}
		private RelayCommand rollbackCommand;

		public RelayCommand RollbackCommand => rollbackCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var selected = SelectedPatient;
			Patients = originalPatients = await patientTableManager.SelectAll();
			foreach (var item in originalPatients)
			{
				if (item.Equals(selected))
				{
					SelectedPatient = null;
					SelectedPatient = item;
					break;
				}
			}
			UnlockCommandExecution();
		}, () => !ExecutionLocked && SelectedPatient != null);
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddPatientVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			if (SelectedPatient != null)
			{
				LockCommandExecution();
				await patientTableManager.UpdateById(SelectedPatient.Id, SelectedPatient);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} modified {SelectedPatient.Account.Id}#{SelectedPatient.Account.Login} patient"
				});
				Patients = originalPatients = await patientTableManager.SelectAll();
				MessengerInstance.Send(new CollectionUpdateMessage<Patient>(this)
				{
					Collection = originalPatients
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedPatient != null);
		private RelayCommand removeCommand;
		public RelayCommand RemoveCommand => removeCommand ??= new RelayCommand(async () =>
		{
			if (SelectedPatient != null)
			{
				LockCommandExecution();
				await patientTableManager.DeleteById(SelectedPatient.Id);
				await accountTableManager.DeleteById(SelectedPatient.Account.Id);
				var patients = await patientTableManager.SelectAll();
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Delete,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} removed {SelectedPatient.Account.Id}#{SelectedPatient.Account.Login} patient"
				});
				MessengerInstance.Send(new CollectionUpdateMessage<Patient>(this)
				{
					Collection = patients
				});
				Patients = originalPatients = patients;
				SearchText = null;
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedPatient != null);
		private RelayCommand recordsCommand;
		public RelayCommand RecordsCommand => recordsCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var recordFetcher = new CustomRecordFetcher();
			MessengerInstance.Send(new CollectionUpdateMessage<Record>(this)
			{
				Collection = await recordFetcher.FetchByPatientId(SelectedPatient.Id)
			});
			MessengerInstance.Send(new ItemUpdateMessage<Patient>(this)
			{
				Item = SelectedPatient
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<PatientRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && SelectedPatient != null);
		private RelayCommand ascendSortCommand;
		public RelayCommand AscendSortCommand => ascendSortCommand ??= new RelayCommand(() =>
		{
			Patients = new HashSet<Patient>(Patients.OrderBy(p => p.Name));
		}, () => !ExecutionLocked);
		private RelayCommand descendSortCommand;
		public RelayCommand DescendSortCommand => descendSortCommand ??= new RelayCommand(() =>
		{
			Patients = new HashSet<Patient>(Patients.OrderByDescending(p => p.Name));
		}, () => !ExecutionLocked);
		private RelayCommand searchCommand;
		public RelayCommand SearchCommand => searchCommand ??= new RelayCommand(() =>
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				Patients = originalPatients;
				return;
			}
			Patients = patientSearchService.Search(originalPatients, SearchText);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			SearchCommand.RaiseCanExecuteChanged();
			DescendSortCommand.RaiseCanExecuteChanged();
			AscendSortCommand.RaiseCanExecuteChanged();
			RecordsCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			RemoveCommand.RaiseCanExecuteChanged();
			RollbackCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
