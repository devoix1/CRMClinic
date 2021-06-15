using CRMBase.Messages;
using CRMBase.Tokens;
using GalaSoft.MvvmLight;

namespace CRMAdmin.ViewModel
{
	public sealed class MainVM : SuperViewModel
	{
		public MainVM()
		{
			CurrentVM = App.Container.GetInstance<LoginVM>();
			MessengerInstance.Register<VMUpdateMessage>(this, MainVMToken.Instance, message =>
			{
				CurrentVM = message.VM as SuperViewModel;
			});
		}
		private SuperViewModel currentVM;
		public SuperViewModel CurrentVM
		{
			get => currentVM;
			set => Set(ref currentVM, value);
		}
		protected override void TriggerCommandExecution() { }
	}
}
