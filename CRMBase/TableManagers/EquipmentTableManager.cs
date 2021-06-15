using CRMBase.Model;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public sealed class EquipmentTableManager : BaseTableManager<Equipment>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByEquipmentId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("EquipmentIdExists", @params);
		}
		public override async Task Insert(Equipment value)
		{
			var @params = new DynamicParameters();
			@params.Add("name", value.Name);
			@params.Add("price", value.Price);
			@params.Add("quantity", value.Quantity);
			await procedureManager.ExecuteVoid("InsertEquipment", @params);
		}
		public override async Task<HashSet<Equipment>> SelectAll()
		{
			return await procedureManager.Execute<Equipment>("SelectAllEquipment");
		}
		public override async Task<Equipment> SelectById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<Equipment>("SelectEquipmentById", @params);
		}
		public override async Task UpdateById(int id, Equipment value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("name", value.Name);
			@params.Add("price", value.Price);
			@params.Add("quantity", value.Quantity);
			await procedureManager.ExecuteVoid("UpdateEquipmentById", @params);
		}
	}
}
