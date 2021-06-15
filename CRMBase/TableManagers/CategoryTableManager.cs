using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace CRMBase.TableManagers
{
	public sealed class CategoryTableManager : BaseTableManager<Category>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByCategoryId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("CategoryIdExists", @params);
		}
		public override async Task Insert(Category value)
		{
			var @params = new DynamicParameters();
			@params.Add("value", value.Value);
			await procedureManager.ExecuteVoid("InsertCategory", @params);
		}
		public override async Task<HashSet<Category>> SelectAll()
		{
			return await procedureManager.Execute<Category>("SelectAllCategory");
		}
		public override async Task<Category> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<Category>("SelectCategoryById", @params);
		}
		public override async Task UpdateById(int id, Category value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("value", value.Value);
			await procedureManager.ExecuteVoid("UpdateCategoryById", @params);
		}
	}
}