using CRMBase.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class RecordTableManager : BaseTableManager<Record>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByRecordId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("RecordIdExists", @params);
		}
		public override async Task Insert(Record value)
		{
			var @params = new DynamicParameters();
			@params.Add("timestamp", value.Timestamp);
			@params.Add("id_record_doctor", value.Doctor.Id);
			@params.Add("id_record_patient", value.Patient.Id);
			@params.Add("id_record_procedure", value.Procedure.Id);
			@params.Add("id_record_procedure_reult", value.ProcedureResult.Id);
			await procedureManager.ExecuteVoid("InsertRecord", @params);
		}
		public override async Task<HashSet<Record>> SelectAll()
		{
			var records = await procedureManager.Execute<Record>("SelectAllRecord");
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
		public override async Task<Record> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			var record = await procedureManager.ExecuteSingle<Record>("SelectRecordById", @params);
			await Task.WhenAll(Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("record_id", record.Id);
				record.Doctor = await procedureManager.ExecuteSingle<Doctor>("SelectDoctorByRecordId", _params);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("record_id", record.Id);
				record.Patient = await procedureManager.ExecuteSingle<Patient>("SelectPatientByRecordId", _params);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("record_id", record.Id);
				record.Procedure = await procedureManager.ExecuteSingle<Procedure>("SelectProcedureByRecordId", _params);
			}), Task.Run(async () =>
			{
				var _params = new DynamicParameters();
				_params.Add("record_id", record.Id);
				record.ProcedureResult = await procedureManager.ExecuteSingle<ProcedureResult>("SelectProcedureResultByRecordId", _params);
			}));
			return record;
		}
		public override async Task UpdateById(int id, Record value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("timestamp", value.Timestamp);
			@params.Add("id_record_doctor", value.Doctor.Id);
			@params.Add("id_record_patient", value.Patient.Id);
			@params.Add("id_record_procedure", value.Procedure.Id);
			@params.Add("id_record_procedure_reult", value.ProcedureResult.Id);
			await procedureManager.ExecuteVoid("UpdateRecordById", @params);
		}
	}
}
