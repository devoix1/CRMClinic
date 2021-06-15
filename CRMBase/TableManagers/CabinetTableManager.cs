using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace CRMBase.TableManagers
{
	public sealed class CabinetTableManager : BaseTableManager<Cabinet>
	{
		public override async Task DeleteById(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			await procedureManager.ExecuteVoid("DeleteByCabinetId", @params);
		}
		public override async Task<bool> Exists(int id)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			return await procedureManager.ExecuteSingle<bool>("CabinetIdExists", @params);
		}
		public override async Task Insert(Cabinet value)
		{
			var @params = new DynamicParameters();
			@params.Add("number", value.Number);
			await procedureManager.ExecuteVoid("InsertCabinet", @params);
		}
		public override async Task<HashSet<Cabinet>> SelectAll()
		{
			var cabinets = await procedureManager.Execute<Cabinet>("SelectAllCabinet");
			var tasks = new HashSet<Task>();
			foreach (var item in cabinets)
			{
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("cabinet_id", item.Id);
					item.Medicines = await procedureManager.Execute<Medicine>("SelectMedicineByCabinetId", @params);
				}));
				tasks.Add(Task.Run(async () =>
				{
					var @params = new DynamicParameters();
					@params.Add("cabinet_id", item.Id);
					item.Equipments = await procedureManager.Execute<Equipment>("SelectEquipmentByCabinetId", @params);
				}));
			}
			Task.WaitAll(tasks.ToArray());
			return cabinets;
		}
		public override async Task<Cabinet> SelectById(int id)
		{
			Cabinet cabinet;
			{
				var @params = new DynamicParameters();
				@params.Add("id", id);
				cabinet = await procedureManager.ExecuteSingle<Cabinet>("SelectCabinetById", @params);
			}
			var _params = new DynamicParameters();
			_params.Add("cabinet_id", cabinet.Id);
			cabinet.Medicines = await procedureManager.Execute<Medicine>("SelectMedicineByCabinetId", _params);
			cabinet.Equipments = await procedureManager.Execute<Equipment>("SelectEquipmentByCabinetId", _params);
			return cabinet;
		}
		public override async Task UpdateById(int id, Cabinet value)
		{
			var @params = new DynamicParameters();
			@params.Add("id", id);
			@params.Add("number", value.Number);
			await procedureManager.ExecuteVoid("UpdateCabinetById", @params);
		}
	}
}
