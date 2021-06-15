using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class AccountMoreInfoVM : SuperViewModel
	{
		public AccountMoreInfoVM()
		{
			MessengerInstance.Register<ItemUpdateMessage<Account>>(this, message =>
			{
				if (message.Sender is AccountVM)
				{
					Account = message.Item;
				}
			});
		}
		private Account account;
		public Account Account
		{
			get => account;
			set
			{
				Set(ref account, value);
				TriggerCommandExecution();
			}
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		public RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await accountTableManager.UpdateById(account.Id, Account);
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Update,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} changed account data of {Account.Id}#{Account.Login}"
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && account.IsValid());
		protected override void TriggerCommandExecution()
		{
			BackCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
