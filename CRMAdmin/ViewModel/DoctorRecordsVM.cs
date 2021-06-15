using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using Dapper;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CRMAdmin.ViewModel
{
	public sealed class DoctorRecordsVM : SuperViewModel
	{
		public DoctorRecordsVM()
		{
			loginTaskManager.AddTask(async () =>
			{
				Cabinets = await cabinetTableManager.SelectAll();
				Patients = await patientTableManager.SelectAll();
				Procedures = await procedureTableManager.SelectAll();
			});
			MessengerInstance.Register<ItemUpdateMessage<Doctor>>(this, message =>
			{
				if (message.Sender is DoctorVM)
				{
					currentDoctor = message.Item;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Record>>(this, message =>
			{
				if (message.Sender is AddDoctorRecordsVM || message.Sender is DoctorVM)
				{
					Records = originalRecords = message.Collection;
				}
			});
		}
		private readonly ISearchService<Record, string> recordSearchService = new RecordSearchService();
		private Doctor currentDoctor;
		private HashSet<Record> originalRecords;
		private HashSet<Record> records;
		public HashSet<Record> Records
		{
			get => records;
			set => Set(ref records, value);
		}
		private Record selectedRecord;
		public Record SelectedRecord
		{
			get => selectedRecord;
			set
			{
				Set(ref selectedRecord, value);
				TriggerCommandExecution();
			}
		}
		private HashSet<Cabinet> cabinets;
		public HashSet<Cabinet> Cabinets
		{
			get => cabinets;
			set => Set(ref cabinets, value);
		}
		private HashSet<Patient> patients;
		public HashSet<Patient> Patients
		{
			get => patients;
			set => Set(ref patients, value);
		}
		private HashSet<Procedure> procedures;
		public HashSet<Procedure> Procedures
		{
			get => procedures;
			set => Set(ref procedures, value);
		}
		private string searchText;
		public string SearchText
		{
			get => searchText;
			set => Set(ref searchText, value);
		}
		private RelayCommand searchCommand;
		public RelayCommand SearchCommand => searchCommand ??= new RelayCommand(() =>
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				Records = originalRecords;
				return;
			}
			Records = recordSearchService.Search(originalRecords, SearchText);
		}, () => !ExecutionLocked);
		private RelayCommand ascendSortCommand;
		public RelayCommand AscendSortCommand => ascendSortCommand ??= new RelayCommand(() =>
		{
			Records = new HashSet<Record>(originalRecords.OrderBy(a => a.Timestamp));
		}, () => !ExecutionLocked);
		private RelayCommand descendSortCommand;
		public RelayCommand DescendSortCommand => descendSortCommand ??= new RelayCommand(() =>
		{
			Records = new HashSet<Record>(originalRecords.OrderByDescending(a => a.Timestamp));
		}, () => !ExecutionLocked);
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			SelectedRecord = null;
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand addCommand;
		public RelayCommand AddCommand => addCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new ItemUpdateMessage<Doctor>(this)
			{
				Item = currentDoctor
			});
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AddDoctorRecordsVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		private RelayCommand diagnoseCommand;
		public RelayCommand DiagnoseCommand => diagnoseCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var @params = new DynamicParameters();
			@params.Add("record_id", SelectedRecord.Id);
			@params.Add("doctor_id", currentDoctor.Id);
			MessengerInstance.Send(new ItemUpdateMessage<Diagnosis>(this)
			{
				Item = await procedureManager.ExecuteSingle<Diagnosis>("SelectDiagnosisByRecordAndDoctorId", @params)
			});
			MessengerInstance.Send(new ItemUpdateMessage<ProcedureResult>(this)
			{
				Item = SelectedRecord.ProcedureResult
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<SetDoctorRecordsDiagnoseVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && SelectedRecord != null);
		private RelayCommand saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var @params = new DynamicParameters();
			@params.Add("id", SelectedRecord.ProcedureResult.Id);
			@params.Add("recipe", SelectedRecord.ProcedureResult.Recipe);
			@params.Add("procudure_timestamp_begin", SelectedRecord.ProcedureResult.ProcudureTimestampBegin);
			@params.Add("procudure_timestamp_end", SelectedRecord.ProcedureResult.ProcudureTimestampEnd);
			await procedureManager.ExecuteVoid("UpdateProcedureResultById", @params);
			var fetcher = new CustomRecordFetcher();
			Records = originalRecords = await fetcher.FetchByDoctorId(currentDoctor.Id);
			SearchText = null;
			UnlockCommandExecution();
		}, () => !ExecutionLocked && SelectedRecord != null);
		private RelayCommand analCommand;
		public RelayCommand AnalCommand => analCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			var @params = new DynamicParameters();
			@params.Add("record_id", SelectedRecord.Id);
			@params.Add("doctor_id", currentDoctor.Id);
			MessengerInstance.Send(new ItemUpdateMessage<Analysis>(this)
			{
				Item = await procedureManager.ExecuteSingle<Analysis>("SelectAnalysisByRecordAndDoctorId", @params)
			});
			MessengerInstance.Send(new ItemUpdateMessage<ProcedureResult>(this)
			{
				Item = SelectedRecord.ProcedureResult
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<SetDoctorRecordsAnalyseVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked && SelectedRecord != null);
		protected override void TriggerCommandExecution()
		{
			AnalCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			DiagnoseCommand.RaiseCanExecuteChanged();
			BackCommand.RaiseCanExecuteChanged();
		}
	}
}