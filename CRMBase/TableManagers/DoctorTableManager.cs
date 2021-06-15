using CRMBase.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class DoctorTableManager : BaseTableManager<Doctor>
	{
		private readonly ITableManager<Cabinet> cabinetTableManager = new CabinetTableManager();
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByDoctorId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("DoctorIdExists", @params);
		}
		public override async Task Insert(Doctor value)
		{
			var @params = new DynamicParameters();
			@params.Add("name", value.Name);
			@params.Add("surname", value.Surname);
			@params.Add("middle_name", value.Middlename);
			@params.Add("email", value.Email);
			@params.Add("phone", value.Phone);
			@params.Add("address", value.Address);
			@params.Add("gender", value.Gender);
			@params.Add("bith_date", value.BirthDate);
			@params.Add("interest_rate", value.InterestRate);
			@params.Add("schedule_daily_begin", value.ScheduleDailyBegin);
			@params.Add("schedule_daily_end", value.ScheduleDailyEnd);
			@params.Add("schedule_weekly_begin", value.ScheduleWeeklyBegin);
			@params.Add("schedule_weekly_end", value.ScheduleWeeklyEnd);
			@params.Add("schedule_rest_begin", value.ScheduleRestBegin);
			@params.Add("schedule_rest_end", value.ScheduleRestEnd);
			@params.Add("id_doctor_account", value.Account.Id);
			@params.Add("id_doctor_category", value.Category.Id);
			@params.Add("id_doctor_cabinet", value.Cabinet.Id);
			await procedureManager.ExecuteVoid("InsertDoctor", @params);
		}
		public override async Task<HashSet<Doctor>> SelectAll()
		{
			var doctors = await procedureManager.Execute<Doctor>("SelectAllDoctor");
			var tasks = new HashSet<Task>();
			foreach (var doctor in doctors)
			{
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("doctor_id", doctor.Id);
					doctor.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByDoctorId", @params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("doctor_id", doctor.Id);
					doctor.Cabinet = await cabinetTableManager.SelectById((await procedureManager.ExecuteSingle<Cabinet>("SelectCabinetByDoctorId", @params)).Id);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("doctor_id", doctor.Id);
					doctor.Procedures = await procedureManager.Execute<Procedure>("SelectAllProceduresByDoctorId", @params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("doctor_id", doctor.Id);
					doctor.Category = await procedureManager.ExecuteSingle<Category>("SelectCategoryByDoctorId", @params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("doctor_id", doctor.Id);
					doctor.Patients = await procedureManager.Execute<Patient>("SelectAllPatientsByDoctorId", @params);
				}));
			}
			Task.WaitAll(tasks.ToArray());
			return doctors;
		}
		public override async Task<Doctor> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			var doctor = await procedureManager.ExecuteSingle<Doctor>("SelectDoctorById", @params);
			Task.WaitAll(Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("doctor_id", doctor.Id);
				doctor.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByDoctorId", _params);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("doctor_id", doctor.Id);
				doctor.Cabinet = await cabinetTableManager.SelectById((await procedureManager.ExecuteSingle<Cabinet>("SelectCabinetByDoctorId", _params)).Id);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("doctor_id", doctor.Id);
				doctor.Procedures = await procedureManager.Execute<Procedure>("SelectAllProceduresByDoctorId", _params);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("doctor_id", doctor.Id);
				doctor.Category = await procedureManager.ExecuteSingle<Category>("SelectCategoryByDoctorId", _params);
			}),
			Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("doctor_id", doctor.Id);
				doctor.Patients = await procedureManager.Execute<Patient>("SelectAllPatientsByDoctorId", _params);
			}));
			return doctor;
		}
		public override async Task UpdateById(int id, Doctor value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("name", value.Name);
			@params.Add("surname", value.Surname);
			@params.Add("middle_name", value.Middlename);
			@params.Add("email", value.Email);
			@params.Add("phone", value.Phone);
			@params.Add("address", value.Address);
			@params.Add("gender", value.Gender);
			@params.Add("bith_date", value.BirthDate);
			@params.Add("interest_rate", value.InterestRate);
			@params.Add("schedule_daily_begin", value.ScheduleDailyBegin);
			@params.Add("schedule_daily_end", value.ScheduleDailyEnd);
			@params.Add("schedule_weekly_begin", value.ScheduleWeeklyBegin);
			@params.Add("schedule_weekly_end", value.ScheduleWeeklyEnd);
			@params.Add("schedule_rest_begin", value.ScheduleRestBegin);
			@params.Add("schedule_rest_end", value.ScheduleRestEnd);
			@params.Add("id_doctor_account", value.Account.Id);
			@params.Add("id_doctor_category", value.Category.Id);
			@params.Add("id_doctor_cabinet", value.Cabinet.Id);
			await procedureManager.ExecuteVoid("UpdateDoctorById", @params);
		}
	}
}
