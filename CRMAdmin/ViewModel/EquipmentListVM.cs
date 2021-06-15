using CRMBase.Messages;
using CRMBase.Model;
using System.Collections.Generic;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public sealed class EquipmentListVM : SuperViewModel
	{
		public EquipmentListVM()
		{
			MessengerInstance.Register<CollectionUpdateMessage<Equipment>>(this, message =>
			{
				if (message.Sender is CabinetVM || message.Sender is EquipmentVM || message.Sender is AddEquipmentVM)
				{
					Equipment = message.Collection;
				}
			});
		}
		private HashSet<Equipment> equipment;
		public HashSet<Equipment> Equipment
		{
			get => equipment;
			set
			{
				if (value == null)
				{
					Set(ref equipment, value);
				}
				else
				{
					Set(ref equipment, new HashSet<Equipment>(value.Where(e => e.Quantity != 0)));
				}
			}
		}
		private Equipment selectedEquipment;
		public Equipment SelectedEquipment
		{
			get => selectedEquipment;
			set
			{
				Set(ref selectedEquipment, value);
				MessengerInstance.Send(new ItemUpdateMessage<Equipment>(this)
				{
					Item = value
				});
			}
		}
		protected override void TriggerCommandExecution()
		{

		}
	}
}
