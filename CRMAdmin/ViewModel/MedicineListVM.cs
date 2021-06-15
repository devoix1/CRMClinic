using CRMBase.Messages;
using CRMBase.Model;
using System.Collections.Generic;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public sealed class MedicineListVM : SuperViewModel
	{
		public MedicineListVM()
		{
			MessengerInstance.Register<CollectionUpdateMessage<Medicine>>(this, message =>
			{
				if (message.Sender is CabinetVM || message.Sender is MedicineVM || message.Sender is AddMedicinesVM)
				{
					Medicines = message.Collection;
				}
			});
		}
		private Medicine selectedMedicine;
		public Medicine SelectedMedicine
		{
			get => selectedMedicine;
			set
			{
				Set(ref selectedMedicine, value);
				MessengerInstance.Send(new ItemUpdateMessage<Medicine>(this)
				{
					Item = value
				});
			}
		}
		private HashSet<Medicine> medicines;
		public HashSet<Medicine> Medicines
		{
			get => medicines;
			set
			{
				if (value == null)
				{
					Set(ref medicines, value);
				}
				else
				{
					Set(ref medicines, new HashSet<Medicine>(value.Where(m => m.Quantity != 0)));
				}
			}
		}
		protected override void TriggerCommandExecution() { }
	}
}
