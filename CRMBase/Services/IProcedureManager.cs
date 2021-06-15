using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public interface IProcedureManager
	{
		Task<HashSet<T>> Execute<T>(string procedureName);
		Task<HashSet<T>> Execute<T>(string procedureName, DynamicParameters parameters);
		Task ExecuteVoid(string procedureName);
		Task ExecuteVoid(string procedureName, DynamicParameters parameters);
		Task<T> ExecuteSingle<T>(string procedureName);
		Task<T> ExecuteSingle<T>(string procedureName, DynamicParameters parameters);
	}
}