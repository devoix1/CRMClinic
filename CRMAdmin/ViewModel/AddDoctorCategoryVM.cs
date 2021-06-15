using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;

namespace CRMAdmin.ViewModel
{
	public sealed class AddDoctorCategoryVM : SuperViewModel
	{
		public AddDoctorCategoryVM()
		{
			loginTaskManager.AddTask(() =>
			{
				Category = new Category();
			});
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private Category category;
		public Category Category
		{
			get => category;
			set => Set(ref category, value);
		}
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			await categoryTableManager.Insert(Category);
			MessengerInstance.Send(new CollectionUpdateMessage<Category>(this)
			{
				Collection = await categoryTableManager.SelectAll()
			});
			await logTableManager.Insert(new Log
			{
				ActionType = ActionType.Create,
				LogType = LogType.Information,
				Message = $"{currentAccount.Id}#{currentAccount.Login} created new doctor category {Category.Value}"
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
			Category = new Category();
		}, () => !ExecutionLocked);
		protected override void TriggerCommandExecution()
		{
			AddCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
