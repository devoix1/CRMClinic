using CRMBase.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class LogTableManager : BaseTableManager<Log>
	{
		public override Task DeleteById(int id) => throw new NotImplementedException();
		public override Task<bool> Exists(int id) => throw new NotImplementedException();
		public override async Task Insert(Log value)
		{
			var @params = new DynamicParameters();
			@params.Add("message", value.Message);
			@params.Add("log_type", value.LogType);
			@params.Add("action_type", value.ActionType);
			@params.Add("timestamp", value.Timestamp);
			await procedureManager.ExecuteVoid("InsertLog", @params);
		}
		public override async Task<HashSet<Log>> SelectAll()
		{
			return await procedureManager.Execute<Log>("SelectAllLogs");
		}
		public override Task<Log> SelectById(int id) => throw new NotImplementedException();
		public override Task UpdateById(int id, Log value) => throw new NotImplementedException();
	}
}
