using CRMBase.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.TableManagers
{
	public interface ITableManager<T>
		where T : HashComparator
	{
		Task<HashSet<T>> SelectAll();
		Task UpdateById(int id, T value);
		Task Insert(T value);
		Task<T> SelectById(int id);
		Task DeleteById(int id);
		Task<bool> Exists(int id);
	}
}
