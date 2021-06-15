using CRMBase.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class PatientTableManager : BaseTableManager<Patient>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByPatientId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("PatientIdExists", @params);
		}
		public override async Task Insert(Patient value)
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
			@params.Add("id_patient_account", value.Account.Id);
			await procedureManager.ExecuteVoid("InsertPatient", @params);
		}
		public override async Task<HashSet<Patient>> SelectAll()
		{
			var result = await procedureManager.Execute<Patient>("SelectAllAPatient");
			var tasks = new HashSet<Task>();
			foreach (var item in result)
			{
				tasks.Add(Task.Run(async () =>
				{
					var _params = new DynamicParameters();
					_params.Add("patient_id", item.Id);
					item.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByPatientId", _params);
				}));
			}
			Task.WaitAll(tasks.ToArray());
			return result;
		}
		public override async Task<Patient> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			var result = await procedureManager.ExecuteSingle<Patient>("SelectPatientById", @params);
			{
				var _params = new DynamicParameters();
				_params.Add("patient_id", result.Id);
				result.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByPatientId", _params);
			}
			return result;
		}
		public override async Task UpdateById(int id, Patient value)
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
			@params.Add("id_patient_account", value.Account.Id);
			await procedureManager.ExecuteVoid("UpdatePatientById", @params);
		}
	}
}
