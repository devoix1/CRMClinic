using CRMBase.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class AdminTableManager : BaseTableManager<Admin>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByAdminId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("AdminIdExists", @params);
		}
		public override async Task Insert(Admin value)
		{
			var @params = new DynamicParameters();
			@params.Add("is_main", value.IsMain);
			@params.Add("name", value.Name);
			@params.Add("surname", value.Surname);
			@params.Add("middle_name", value.Middlename);
			@params.Add("email", value.Email);
			@params.Add("phone", value.Phone);
			@params.Add("address", value.Address);
			@params.Add("gender", value.Gender);
			@params.Add("bith_date", value.BirthDate);
			@params.Add("id_admin_account", value.Account.Id);
			await procedureManager.ExecuteVoid("InsertAdmin", @params);
		}
		public override async Task<HashSet<Admin>> SelectAll()
		{
			ITableManager<Account> accountTableManager = new AccountTableManager();
			var admins = await procedureManager.Execute<Admin>("SelectAllAdmin");
			var accounts = await accountTableManager.SelectAll();
			var tasks = new HashSet<Task>();
			foreach (var item in admins)
			{
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("admin_id", item.Id);
					item.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByAdminId", @params);
				}));
			}
			Task.WaitAll(tasks.ToArray());
			return admins;
		}
		public override async Task<Admin> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			var admin = await procedureManager.ExecuteSingle<Admin>("SelectAdminById", @params);
			if (admin != null)
			{
				var _params = new DynamicParameters();
				_params.Add("admin_id", admin.Id);
				admin.Account = await procedureManager.ExecuteSingle<Account>("SelectAccountByAdminId", @params);
			}
			return admin;
		}
		public override async Task UpdateById(int id, Admin value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("is_main", value.IsMain);
			@params.Add("name", value.Name);
			@params.Add("surname", value.Surname);
			@params.Add("middle_name", value.Middlename);
			@params.Add("email", value.Email);
			@params.Add("phone", value.Phone);
			@params.Add("address", value.Address);
			@params.Add("gender", value.Gender);
			@params.Add("bith_date", value.BirthDate);
			@params.Add("id_admin_account", value.Account.Id);
			await procedureManager.ExecuteVoid("UpdateAdminById", @params);
		}
	}
}
