using System.Collections.Generic;
using System.Threading.Tasks;
using CRMBase.Services;
using CRMBase.Model;

namespace CRMBase.TableManagers
{
	public abstract class BaseTableManager<T> : ITableManager<T>
		where T : HashComparator
	{
		protected IProcedureManager procedureManager = new ProcedureManager();
		public abstract Task DeleteById(int id);
		public abstract Task<bool> Exists(int id);
		public abstract Task Insert(T value);
		public abstract Task<HashSet<T>> SelectAll();
		public abstract Task<T> SelectById(int id);
		public abstract Task UpdateById(int id, T value);
	}
}
