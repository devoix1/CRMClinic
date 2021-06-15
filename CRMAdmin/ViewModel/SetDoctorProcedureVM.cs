using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMAdmin.ViewModel
{
	public sealed class SetDoctorProcedureVM : SuperViewModel
	{
		public SetDoctorProcedureVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Procedures = await procedureTableManager.SelectAll();
			});
			MessengerInstance.Register<CollectionUpdateMessage<Procedure>>(this, message =>
			{
				if (message.Sender is ProcedureVM || message.Sender is AddDoctorVM)
				{
					Procedures = message.Collection;
				}
			});
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddDoctorVM>()
			}, DashboardVMToken.Instance);
		});
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			if (SelectedProcedure != null)
			{
				var res = Procedures.Remove(SelectedProcedure);
				MessengerInstance.Send(new ItemUpdateMessage<Procedure>(this)
				{
					Item = SelectedProcedure
				});
				SelectedProcedure = null;
				Procedures = new HashSet<Procedure>(Procedures);
			}
		});
		private HashSet<Procedure> procedures;
		public HashSet<Procedure> Procedures
		{
			get => procedures;
			set => Set(ref procedures, value);
		}
		private Procedure selectedProcedure;
		public Procedure SelectedProcedure
		{
			get => selectedProcedure;
			set => Set(ref selectedProcedure, value);
		}
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddDoctorVM>()
			}, DashboardVMToken.Instance);
		});
		protected override void TriggerCommandExecution()
		{

		}
	}
}
