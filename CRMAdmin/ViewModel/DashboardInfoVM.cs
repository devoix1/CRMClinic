using CRMBase.Messages;
using CRMBase.Model;
using System.Collections.Generic;

namespace CRMAdmin.ViewModel
{
	public sealed class DashboardInfoVM : SuperViewModel
	{
		public DashboardInfoVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				DoctorCount = (await doctorTableManager.SelectAll())?.Count ?? 0;
				PatientCount = (await patientTableManager.SelectAll())?.Count ?? 0;
				MedicineCount = (await medicineTableManager.SelectAll())?.Count ?? 0;
				EquipmentCount = (await equipmentTableManager.SelectAll())?.Count ?? 0;
				Logs = await logTableManager.SelectAll();
			});
			MessengerInstance.Register<CollectionUpdateMessage<Doctor>>(this, message =>
			{
				if (message.Sender is DoctorVM || message.Sender is AddDoctorVM)
				{
					DoctorCount = message.Collection?.Count ?? 0;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Patient>>(this, message =>
			{
				if (message.Sender is PatientVM || message.Sender is AddPatientVM)
				{
					PatientCount = message.Collection.Count;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Medicine>>(this, message =>
			{
				if (message.Sender is AddMedicinesVM)
				{
					MedicineCount = message.Collection?.Count ?? 0;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Equipment>>(this, message =>
			{
				if (message.Sender is AddEquipmentVM)
				{
					EquipmentCount = message.Collection?.Count ?? 0;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Log>>(this, message =>
			{
				if (message.Sender is DashboardVM)
				{
					Logs = message.Collection;
				}
			});
		}
		private HashSet<Log> logs;
		public HashSet<Log> Logs
		{
			get => logs;
			set => Set(ref logs, value);
		}
		private int doctorCount;
		public int DoctorCount
		{
			get => doctorCount;
			set => Set(ref doctorCount, value);
		}
		private int medicineCount;
		public int MedicineCount
		{
			get => medicineCount;
			set => Set(ref medicineCount, value);
		}
		private int equipmentCount;
		public int EquipmentCount
		{
			get => equipmentCount;
			set => Set(ref equipmentCount, value);
		}
		private int patientCount;
		public int PatientCount
		{
			get => patientCount;
			set => Set(ref patientCount, value);
		}
		protected override void TriggerCommandExecution()
		{

		}
	}
}
