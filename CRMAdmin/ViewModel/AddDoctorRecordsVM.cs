using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;

namespace CRMAdmin.ViewModel
{
	public sealed class AddDoctorRecordsVM : SuperViewModel
	{
		public AddDoctorRecordsVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Procedures = await procedureTableManager.SelectAll();
				Patients = await patientTableManager.SelectAll();
			});
			MessengerInstance.Register<CollectionUpdateMessage<Patient>>(this, message =>
			{
				if (message.Sender is AddPatientVM || message.Sender is PatientVM)
				{
					Patients = message.Collection;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Procedure>>(this, message =>
			{
				if (message.Sender is ProcedureVM)
				{
					Procedures = message.Collection;
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<Doctor>>(this, message =>
			{
				if (message.Sender is DoctorRecordsVM)
				{
					currentDoctor = message.Item;
				}
			});
		}
		private Doctor currentDoctor;
		private HashSet<Procedure> procedures;
		public HashSet<Procedure> Procedures
		{
			get => procedures;
			set => Set(ref procedures, value);
		}
		private HashSet<Patient> patients;
		public HashSet<Patient> Patients
		{
			get => patients;
			set => Set(ref patients, value);
		}
		private Patient selectedPatient;
		public Patient SelectedPatient
		{
			get => selectedPatient;
			set
			{
				Set(ref selectedPatient, value);
				TriggerCommandExecution();
			}
		}
		private Procedure selectedProcedure;
		public Procedure SelectedProcedure
		{
			get => selectedProcedure;
			set
			{
				Set(ref selectedProcedure, value);
				TriggerCommandExecution();
			}
		}
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			SelectedProcedure = null;
			SelectedPatient = null;
			currentDoctor = null;
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			{
				var @params = new DynamicParameters();
				@params.Add("id_doctor", currentDoctor.Id);
				@params.Add("id_procedure", SelectedProcedure.Id);
				await procedureManager.ExecuteVoid("InsertDoctorProcedures", @params);
			}
			{
				var @params = new DynamicParameters();
				@params.Add("id_doctor", currentDoctor.Id);
				@params.Add("id_patient", SelectedPatient.Id);
				await procedureManager.ExecuteVoid("InsertDoctorPatient", @params);
			}
			var procedureResult = new ProcedureResult();
			{
				var @params = new DynamicParameters();
				@params.Add("recipe", null);
				@params.Add("procudure_timestamp_begin", null);
				@params.Add("procudure_timestamp_end", null);
				await procedureManager.ExecuteVoid("InsertProcedureResult", @params);
			}
			procedureResult.Id = await procedureManager.ExecuteSingle<int>("LastProcedureResultId");
			await recordTableManager.Insert(new Record
			{
				Doctor = currentDoctor,
				Patient = SelectedPatient,
				Procedure = SelectedProcedure,
				ProcedureResult = procedureResult,
				Timestamp = DateTime.Now
			});
			{
				var fetcher = new CustomRecordFetcher();
				MessengerInstance.Send(new CollectionUpdateMessage<Record>(this)
				{
					Collection = await fetcher.FetchByDoctorId(currentDoctor.Id)
				});
			}
			UnlockCommandExecution();
			if (BackCommand.CanExecute(null))
			{
				BackCommand.Execute(null);
			}
		}, () => !ExecutionLocked && SelectedPatient != null && SelectedProcedure != null);
		protected override void TriggerCommandExecution()
		{
			AddCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}
