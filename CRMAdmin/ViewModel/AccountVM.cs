using CRMBase.Messages;
using CRMBase.Tokens;
using CRMBase.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using CRMBase.Services;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAdmin.ViewModel
{
	public sealed class AccountVM : SuperViewModel
	{
		public AccountVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Admins = originalAdmins = await adminTableManager.SelectAll();
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Admin>>(this, message =>
			{
				if (message.Sender is AddAccountVM)
				{
					Admins = originalAdmins = message.Collection;
					SearchText = null;
				}
			});
		}
		private readonly AvatarManager avatarManager = App.Container.GetInstance<AvatarManager>();
		private readonly ISearchService<Admin, string> adminSearchService = new AdminSearchService();
		public List<string> Genders { get; private set; }
		private HashSet<Admin> admins;
		public HashSet<Admin> Admins
		{
			get => admins;
			set => Set(ref admins, value);
		}
		private Admin selectedAdmin;
		public Admin SelectedAdmin
		{
			get => selectedAdmin;
			set
			{
				Set(ref selectedAdmin, value);
				TriggerCommandExecution();
			}
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddAccountVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand moreInfoCommand;
		public RelayCommand MoreInfoCommand => moreInfoCommand ??= new RelayCommand(() =>
		{
			if (SelectedAdmin == null)
			{
				return;
			}
			MessengerInstance.Send(new ItemUpdateMessage<Account>(this)
			{
				Item = SelectedAdmin.Account
			});
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountMoreInfoVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && SelectedAdmin != null);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			if (SelectedAdmin != null)
			{
				LockCommandExecution();
				await adminTableManager.UpdateById(SelectedAdmin.Id, SelectedAdmin);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Update,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} modified account {SelectedAdmin.Id}#{SelectedAdmin.Account.Login}"
				});
				Admins = originalAdmins = await adminTableManager.SelectAll();
				SearchText = null;
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedAdmin != null);
		private RelayCommand ascendSortCommand;
		public RelayCommand AscendSortCommand => ascendSortCommand ??= new RelayCommand(() =>
		{
			Admins = new HashSet<Admin>(originalAdmins.OrderBy(a => a.Name));
		});
		private RelayCommand descendSortCommand;
		public RelayCommand DescendSortCommand => descendSortCommand ??= new RelayCommand(() =>
		{
			Admins = new HashSet<Admin>(originalAdmins.OrderByDescending(a => a.Name));
		});
		private string searchText;
		public string SearchText
		{
			get => searchText?.ToLower() ?? string.Empty;
			set => Set(ref searchText, value);
		}
		private HashSet<Admin> originalAdmins;
		private RelayCommand searchCommand;
		public RelayCommand SearchCommand => searchCommand ??= new RelayCommand(() =>
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				Admins = originalAdmins;
				return;
			}
			Admins = adminSearchService.Search(originalAdmins, SearchText);
		});
		private RelayCommand removeCommand;
		public RelayCommand RemoveCommand => removeCommand ??= new RelayCommand(async () =>
		{
			if (SelectedAdmin != null)
			{
				LockCommandExecution();
				await adminTableManager.DeleteById(SelectedAdmin.Id);
				await accountTableManager.DeleteById(SelectedAdmin.Account.Id);
				await logTableManager.Insert(new Log
				{
					ActionType = ActionType.Delete,
					LogType = LogType.Information,
					Message = $"{currentAccount.Id}#{currentAccount.Login} deleted account {SelectedAdmin.Id}#{SelectedAdmin.Account.Login}"
				});
				Admins = originalAdmins = await adminTableManager.SelectAll();
				UnlockCommandExecution();
			}
		}, () => !ExecutionLocked && SelectedAdmin != null && (SelectedAdmin.IsMain != null && !(bool)SelectedAdmin.IsMain));
		private RelayCommand uploadPhotoCommand;
		public RelayCommand UploadPhotoCommand => uploadPhotoCommand ??= new RelayCommand(async () =>
		{
			if (SelectedAdmin != null)
			{
				LockCommandExecution();
				var fileDialog = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
				};
				var result = fileDialog.ShowDialog();
				if (result == false)
				{
					return;
				}
				var @params = new DynamicParameters();
				@params.Add("admin_id", SelectedAdmin.Id);
				var acc = await procedureManager.ExecuteSingle<Account>("SelectAccountByAdminId", @params);
				if (acc != null)
				{
					await avatarManager.UploadAvatar(acc, fileDialog.FileName);
					if (SelectedAdmin.Account.Equals(currentAccount))
					{
						MessengerInstance.Send(new ItemUpdateMessage<Account>(this)
						{
							Item = currentAccount
						});
					}
					await logTableManager.Insert(new Log
					{
						ActionType = ActionType.Internal,
						LogType = LogType.Information,
						Message = $"{currentAccount.Id}#{currentAccount.Login} uploaded a new avatar for {SelectedAdmin.Id}#{SelectedAdmin.Account.Login} account"
					});
				}
				UnlockCommandExecution();
			}
		});
		protected override void TriggerCommandExecution()
		{
			RemoveCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			MoreInfoCommand.RaiseCanExecuteChanged();
			UploadPhotoCommand.RaiseCanExecuteChanged();
		}
	}
}
