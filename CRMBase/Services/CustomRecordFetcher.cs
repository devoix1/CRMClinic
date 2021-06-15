using CRMBase.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public class CustomRecordFetcher
	{
		private IProcedureManager procedureManager = new ProcedureManager();
		public async Task<HashSet<Record>> FetchByDoctorId(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("doctor_id", id);
			var records = await procedureManager.Execute<Record>("AllRecordsByDoctorId", @params);
			var tasks = new HashSet<Task>();
			foreach (var record in records)
			{
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Doctor = await procedureManager.ExecuteSingle<Doctor>("SelectDoctorByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Patient = await procedureManager.ExecuteSingle<Patient>("SelectPatientByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Procedure = await procedureManager.ExecuteSingle<Procedure>("SelectProcedureByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.ProcedureResult = await procedureManager.ExecuteSingle<ProcedureResult>("SelectProcedureResultByRecordId", _params);
				}));
			}
			await Task.WhenAll(tasks);
			return records;
		}
		public async Task<HashSet<Record>> FetchByPatientId(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("patient_id", id);
			var records = await procedureManager.Execute<Record>("AllRecordsByPatientId", @params);
			var tasks = new HashSet<Task>();
			foreach (var record in records)
			{
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Doctor = await procedureManager.ExecuteSingle<Doctor>("SelectDoctorByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Patient = await procedureManager.ExecuteSingle<Patient>("SelectPatientByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.Procedure = await procedureManager.ExecuteSingle<Procedure>("SelectProcedureByRecordId", _params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("record_id", record.Id);
					record.ProcedureResult = await procedureManager.ExecuteSingle<ProcedureResult>("SelectProcedureResultByRecordId", _params);
				}));
			}
			await Task.WhenAll(tasks);
			return records;
		}
	}
}
