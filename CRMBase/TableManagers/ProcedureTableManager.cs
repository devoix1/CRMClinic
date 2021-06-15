using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace CRMBase.TableManagers
{
	public sealed class ProcedureTableManager : BaseTableManager<Procedure>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByProcedureId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("ProcedureIdExists", @params);
		}
		public override async Task Insert(Procedure value)
		{
			var @params = new DynamicParameters();
			@params.Add("name", value.Name);
			@params.Add("description", value.Description);
			@params.Add("price", value.Price);
			@params.Add("duration", value.Duration);
			await procedureManager.ExecuteVoid("InsertProcedure", @params);
		}
		public override async Task<HashSet<Procedure>> SelectAll()
		{
			return await procedureManager.Execute<Procedure>("SelectAllProcedure");
		}
		public override async Task<Procedure> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<Procedure>("SelectProcedureById", @params);
		}
		public override async Task UpdateById(int id, Procedure value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("name", value.Name);
			@params.Add("description", value.Description);
			@params.Add("price", value.Price);
			@params.Add("duration", value.Duration);
			await procedureManager.ExecuteVoid("UpdateProcedureById", @params);
		}
	}
}