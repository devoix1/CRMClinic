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
	public sealed class DoctorVM : SuperViewModel
	{
		public DoctorVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Categories = await categoryTableManager.SelectAll();
				Cabinets = await cabinetTableManager.SelectAll();
				Doctors = originalDoctors = await doctorTableManager.SelectAll();
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Category>>(this, message =>
			{
				Categories = message.Collection;
			});
			MessengerInstance.Register<CollectionUpdateMessage<Cabinet>>(this, message =>
			{
				Cabinets = message.Collection;
			});
			MessengerInstance.Register<CollectionUpdateMessage<Doctor>>(this, message =>
			{
				if (message.Sender is AddDoctorVM)
				{
					Doctors = originalDoctors = message.Collection;
					SearchText = null;
				}
			});
		}
		private readonly ISearchService<Doctor, string> doctorSearchService = new DoctorSearchService();
		public List<string> Genders { get; private set; }
		private HashSet<Doctor> doctors;
		public HashSet<Doctor> Doctors
		{
			get => doctors;
			set => Set(ref doctors, value);
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
		private Doctor selectedDoctor;
		public Doctor SelectedDoctor
		{
			get => selectedDoctor;
			set
			{
				Set(ref selectedDoctor, value);
				TriggerCommandExecution();
			}
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddDoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand addDoctorCategoryCommand;
		public RelayCommand AddDoctorCategoryCommand => addDoctorCategoryCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddDoctorCategoryVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand moreInfoCommand;
		public RelayCommand MoreInfoCommand => moreInfoCommand ??= new RelayCommand(async () =>
		{
			if (SelectedDoctor != null)
			{
				LockCommandExecution();
				MessengerInstance.Send(new ItemUpdateMessage<Doctor>(this)
				{
					Item = SelectedDoctor
				});
				MessengerInstance.Send(new ItemUpdateMessage<Account>(this)
				{
					Item = await accountTableManager.SelectById(SelectedDoctor.Account.Id)
				});
				UnlockCommandExecution();
				MessengerInstance.Send(new VMUpdateMessage(this)
				{
					VM = App.Container.GetInstance<DoctorMoreInfoVM>()
				}, DashboardVMToken.Instance);
			}
		}, () => !ExecutionLocked && SelectedDoctor != null);
		private RelayCommand removeCommand;
		public RelayCommand RemoveCommand => removeCommand ??= new RelayCommand(async () =>
		{
			if (SelectedDoctor != null)
			{
				LockCommandExecution();
				await doctorTableManager.DeleteById(SelectedDoctor.Id);
				await accountTableManager.DeleteById(SelectedDoctor.Account.Id);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Delete,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} deleted {SelectedDoctor.Account.Id}#{SelectedDoctor.Account.Login} doctor"
				});
				Doctors = originalDoctors = await doctorTableManager.SelectAll();
				MessengerInstance.Send(new CollectionUpdateMessage<Doctor>(this)
				{
					Collection = Doctors
				});
				SearchText = null;
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedDoctor != null);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			if (SelectedDoctor != null)
			{
				LockCommandExecution();
				SearchText = null;
				await doctorTableManager.UpdateById(SelectedDoctor.Id, SelectedDoctor);
				Doctors = await doctorTableManager.SelectAll();
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} modified {SelectedDoctor.Account.Id}#{SelectedDoctor.Account.Login} doctor"
				});
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedDoctor != null && SelectedDoctor.IsValid());
		private HashSet<Doctor> originalDoctors;
		private RelayCommand ascendSortCommand;
		public RelayCommand AscendSortCommand => ascendSortCommand ??= new RelayCommand(() =>
		{
			Doctors = new HashSet<Doctor>(originalDoctors.OrderBy(d => d.Name));
		}, () => !ExecutionLocked);
		private RelayCommand descendSortCommand;
		public RelayCommand DescendSortCommand => descendSortCommand ??= new RelayCommand(() =>
		{
			Doctors = new HashSet<Doctor>(originalDoctors.OrderByDescending(d => d.Name));
		}, () => !ExecutionLocked);
		private string searchText;
		public string SearchText
		{
			get => searchText?.ToLower() ?? string.Empty;
			set => Set(ref searchText, value);
		}
		private RelayCommand searchCommand;
		public RelayCommand SearchCommand => searchCommand ??= new RelayCommand(() =>
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				Doctors = originalDoctors;
				return;
			}
			Doctors = doctorSearchService.Search(originalDoctors, SearchText);
		}, () => !ExecutionLocked);
		private RelayCommand recordsCommand;
		public RelayCommand RecordsCommand => recordsCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var fetcher = new CustomRecordFetcher();
			MessengerInstance.Send(new CollectionUpdateMessage<Record>(this)
			{
				Collection = await fetcher.FetchByDoctorId(SelectedDoctor.Id)
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new ItemUpdateMessage<Doctor>(this)
			{
				Item = SelectedDoctor
			});
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && SelectedDoctor != null);
		private RelayCommand rollbackCommand;
		public RelayCommand RollbackCommand => rollbackCommand ??= new RelayCommand(async () =>
		{
			if (SelectedDoctor != null)
			{
				LockCommandExecution();
				await doctorTableManager.SelectById(SelectedDoctor.Id);
				var selected = SelectedDoctor;
				Doctors = await doctorTableManager.SelectAll();
				foreach (var item in Doctors)
				{
					if (item.Equals(selected))
					{
						SelectedDoctor = item;
						break;
					}
				}
				SearchText = null;
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			RollbackCommand.RaiseCanExecuteChanged();
			RecordsCommand.RaiseCanExecuteChanged();
			SearchCommand.RaiseCanExecuteChanged();
			DescendSortCommand.RaiseCanExecuteChanged();
			AscendSortCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			RemoveCommand.RaiseCanExecuteChanged();
			MoreInfoCommand.RaiseCanExecuteChanged();
			AddDoctorCategoryCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
		}
	}
}