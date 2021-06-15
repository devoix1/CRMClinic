using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace CRMBase.TableManagers
{
	public sealed class AccountTableManager : BaseTableManager<Account>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByAccountId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("AccountIdExists", @params);
		}
		public override async Task Insert(Account value)
		{
			var @params = new DynamicParameters();
			@params.Add("account_type", value.Type);
			@params.Add("login", value.Login);
			@params.Add("password", value.Password);
			await procedureManager.ExecuteVoid("InsertAccount", @params);
		}
		public override async Task<HashSet<Account>> SelectAll()
		{
			return await procedureManager.Execute<Account>("SelectAllAccount");
		}
		public override async Task<Account> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<Account>("SelectAccountById", @params);
		}
		public override async Task UpdateById(int id, Account value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("account_type", value.Type);
			@params.Add("login", value.Login);
			@params.Add("password", value.Password);
			await procedureManager.ExecuteVoid("UpdateAccountById", @params);
		}
	}
}
