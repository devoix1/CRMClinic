using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class DoctorMoreInfoVM : SuperViewModel
	{
		public DoctorMoreInfoVM()
		{
			MessengerInstance.Register<ItemUpdateMessage<Account>>(this, message =>
			{
				if (message.Sender is DoctorVM)
				{
					Account = message.Item;
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<Doctor>>(this, message =>
			{
				if (message.Sender is DoctorVM)
				{
					Doctor = message.Item;
				}
			});
		}
		private Account account;
		public Account Account
		{
			get => account;
			set => Set(ref account, value);
		}
		private Doctor doctor;
		public Doctor Doctor
		{
			get => doctor;
			set => Set(ref doctor, value);
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await accountTableManager.UpdateById(Account.Id, Account);
			await doctorTableManager.UpdateById(Doctor.Id, Doctor);
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Update,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} updated schedule information and/or login data for {Account.Id}#{Account.Login} doctor"
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			SaveCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
