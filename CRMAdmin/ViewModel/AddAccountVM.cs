using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace CRMAdmin.ViewModel
{
	public sealed class AddAccountVM : SuperViewModel
	{
		public AddAccountVM()
		{
			loginTaskManager.AddTask(() =>
			{
				if (Genders == null || Genders.Count == 0)
				{
					Genders = ConfigReader.Instance.Read("GenderList", new GenderListValueReader());
				}
				Admin = new Admin();
				Account = new Account();
			});
		}
		public List<string> Genders { get; set; }
		private Account account;
		public Account Account
		{
			get => account;
			set => Set(ref account, value);
		}
		private Admin admin;
		public Admin Admin
		{
			get => admin;
			set => Set(ref admin, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			Account.Type = AccountType.Admin;
			await accountTableManager.Insert(Account);
			var lastAccountId = await procedureManager.ExecuteSingle<int>("LastAccountId");
			Account.Id = lastAccountId;
			Admin.Account = Account;
			Admin.IsMain = false;
			await adminTableManager.Insert(Admin);
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} created new account {Account.Id}#{Account.Login}"
			});
			MessengerInstance.Send(new CollectionUpdateMessage<Admin>(this)
			{
				Collection = await adminTableManager.SelectAll()
			});
			Admin = new Admin();
			Account = new Account();
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			AddCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}