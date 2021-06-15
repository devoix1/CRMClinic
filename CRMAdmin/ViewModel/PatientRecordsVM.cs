using CRMBase.Messages;
using CRMBase.Model;
using CRMBase.Services;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public sealed class PatientRecordsVM : SuperViewModel
	{
		public PatientRecordsVM()
		{
			MessengerInstance.Register<ItemUpdateMessage<Patient>>(this, message =>
			{
				if (message.Sender is PatientVM)
				{
					currentPatient = message.Item;
				}
			});
			MessengerInstance.Register<CollectionUpdateMessage<Record>>(this, message =>
			{
				if (message.Sender is PatientVM)
				{
					Records = originalRecords = message.Collection;
				}
			});
		}
		private readonly ISearchService<Record, string> recordSearchService = new RecordSearchService();
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
				DoctorFullname = $"{value.Doctor.Surname} {value.Doctor.Name} {value.Doctor.Middlename}";
			}
		}
		private string doctorFullname;
		public string DoctorFullname
		{
			get => doctorFullname;
			set => Set(ref doctorFullname, value);
		}
		private Patient currentPatient;
		private RelayCommand backCommand;
		public RelayCommand BackCommand => backCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<PatientVM>()
			}, DashboardVMToken.Instance);
		});
		private RelayCommand ascendSortCommand;
		public RelayCommand AscendSortCommand => ascendSortCommand ??= new RelayCommand(() =>
		{
			Records = new HashSet<Record>(Records.OrderBy(r => r.Timestamp));
		});
		private RelayCommand descendSortCommand;
		public RelayCommand DescendSortCommand => descendSortCommand ??= new RelayCommand(() =>
		{
			Records = new HashSet<Record>(Records.OrderByDescending(r => r.Timestamp));
		});
		private RelayCommand searchCommand;
		public RelayCommand SearchCommand => searchCommand ??= new RelayCommand(() =>
		{
			if (string.IsNullOrEmpty(searchText))
			{
				Records = originalRecords;
				return;
			}
			Records = recordSearchService.Search(originalRecords, SearchText);
		});
		private string searchText;
		public string SearchText
		{
			get => searchText;
			set => Set(ref searchText, value);
		}
		protected override void TriggerCommandExecution() => throw new System.NotImplementedException();
	}
}
