using CRMAdmin.ViewModel;
using CRMBase.Services;
using CRMBase.TableManagers;
using Dapper;
using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace CRMAdmin
{
	public partial class App : Application
	{
		static App()
		{
			Container = new Container();
		}
		public static Container Container { get; }
		protected override void OnStartup(StartupEventArgs e)
		{
			Init();
			base.OnStartup(e);
		}
		private static void Init()
		{
			RegisterVMs();
			RegisterServices();
			Container.Verify();
			try
			{
				var dbManager = new DatabaseManager();
				dbManager.CreateIfNotExists($@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\{ConfigReader.Instance.Read("DatabaseName")}.sql", ConfigReader.Instance.Read("DatabaseName"));
			}
			catch (Exception ex) when(!(ex is FileNotFoundException) && !(ex is DirectoryNotFoundException))
			{
				MessageBox.Show($"An error occurred; \n{ex.Message}");
				Current.Shutdown();
			}
			Task.Run(async () =>
			{
				IProcedureManager procManager = new ProcedureManager();
				var result = await procManager.ExecuteSingle<int>("CountAdminAccounts");
				if (result == 0)
				{
					var defaultAccount = ConfigReader.Instance.Read("DefaultAccount", new AccountValueReader());
					var accountParams = new DynamicParameters();
					accountParams.Add("account_type", defaultAccount.Type);
					accountParams.Add("login", defaultAccount.Login);
					accountParams.Add("password", defaultAccount.Password);
					await procManager.ExecuteVoid("InsertAccount", accountParams);
					var defaultAdmin = ConfigReader.Instance.Read("DefaultAdmin", new AdminValueReader());
					var adminParams = new DynamicParameters();
					adminParams.Add("is_main", defaultAdmin.IsMain);
					adminParams.Add("name", defaultAdmin.Name);
					adminParams.Add("surname", defaultAdmin.Surname);
					adminParams.Add("middle_name", defaultAdmin.Middlename);
					adminParams.Add("email", defaultAdmin.Email);
					adminParams.Add("phone", defaultAdmin.Phone);
					adminParams.Add("address", defaultAdmin.Address);
					adminParams.Add("gender", defaultAdmin.Gender);
					adminParams.Add("bith_date", defaultAdmin.BirthDate);
					adminParams.Add("id_admin_account", await procManager.ExecuteSingle<int>("LastAccountId"));
					await procManager.ExecuteVoid("InsertAdmin", adminParams);
				}
			});
		}
		private static void RegisterServices()
		{
			Container.RegisterSingleton<Messenger>();
			Container.RegisterSingleton(() =>
			{
				return new AvatarManager(Directory.GetCurrentDirectory());
			});
			Container.RegisterSingleton<AppTaskManager>();
			Container.RegisterSingleton<AccountTableManager>();
			Container.RegisterSingleton<AdminTableManager>();
			Container.RegisterSingleton<CabinetTableManager>();
			Container.RegisterSingleton<CategoryTableManager>();
			Container.RegisterSingleton<DoctorTableManager>();
			Container.RegisterSingleton<EquipmentTableManager>();
			Container.RegisterSingleton<LogTableManager>();
			Container.RegisterSingleton<MedicineTableManager>();
			Container.RegisterSingleton<PatientTableManager>();
			Container.RegisterSingleton<ProcedureManager>();
			Container.RegisterSingleton<ProcedureTableManager>();
			Container.RegisterSingleton<RecordTableManager>();
		}
		private static void RegisterVMs()
		{
			Container.RegisterSingleton<MainVM>();
			Container.RegisterSingleton<LoginVM>();
			Container.RegisterSingleton<DashboardVM>();
			Container.RegisterSingleton<DashboardInfoVM>();
			Container.RegisterSingleton<PatientVM>();
			Container.RegisterSingleton<DoctorVM>();
			Container.RegisterSingleton<AccountVM>();
			Container.RegisterSingleton<CabinetVM>();
			Container.RegisterSingleton<MedicineVM>();
			Container.RegisterSingleton<ProcedureVM>();
			Container.RegisterSingleton<EquipmentVM>();
			Container.RegisterSingleton<AddDoctorVM>();
			Container.RegisterSingleton<DoctorMoreInfoVM>();
			Container.RegisterSingleton<AddPatientVM>();
			Container.RegisterSingleton<PatientRecordsVM>();
			Container.RegisterSingleton<AddAccountVM>();
			Container.RegisterSingleton<AddDoctorCategoryVM>();
			Container.RegisterSingleton<AccountMoreInfoVM>();
			Container.RegisterSingleton<DoctorRecordsVM>();
			Container.RegisterSingleton<AddEquipmentVM>();
			Container.RegisterSingleton<AddMedicinesVM>();
			Container.RegisterSingleton<AddCabinetVM>();
			Container.RegisterSingleton<EquipmentListVM>();
			Container.RegisterSingleton<MedicineListVM>();
			Container.RegisterSingleton<SetDoctorProcedureVM>();
			Container.RegisterSingleton<ShowCabinetInfoVM>();
			Container.RegisterSingleton<SetDoctorRecordsAnalyseVM>();
			Container.RegisterSingleton<SetDoctorRecordsDiagnoseVM>();
			Container.RegisterSingleton<AddDoctorRecordsVM>();
		}
	}
}
