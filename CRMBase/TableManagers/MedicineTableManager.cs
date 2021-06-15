using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace CRMBase.TableManagers
{
	public sealed class MedicineTableManager : BaseTableManager<Medicine>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByMedicineId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("MedicineIdExists", @params);
		}
		public override async Task Insert(Medicine value)
		{
			var @params = new DynamicParameters();
			@params.Add("name", value.Name);
			@params.Add("price", value.Price);
			@params.Add("doses", value.Doses);
			@params.Add("quantity", value.Quantity);
			await procedureManager.ExecuteVoid("InsertMedicine", @params);
		}
		public override async Task<HashSet<Medicine>> SelectAll()
		{
			return await procedureManager.Execute<Medicine>("SelectAllMedicine");
		}
		public override async Task<Medicine> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<Medicine>("SelectMedicineById", @params);
		}
		public override async Task UpdateById(int id, Medicine value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("name", value.Name);
			@params.Add("price", value.Price);
			@params.Add("doses", value.Doses);
			@params.Add("quantity", value.Quantity);
			await procedureManager.ExecuteVoid("UpdateMedicineById", @params);
		}
	}
}