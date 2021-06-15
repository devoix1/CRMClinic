using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public abstract class BaseProcedureManager : IProcedureManager
	{
		protected string connectionString = ConfigReader.Instance.Read("ConnectionString");
		public abstract Task<HashSet<T>> Execute<T>(string procedureName);
		public abstract Task<HashSet<T>> Execute<T>(string procedureName, DynamicParameters parameters);
		public abstract Task<T> ExecuteSingle<T>(string procedureName);
		public abstract Task<T> ExecuteSingle<T>(string procedureName, DynamicParameters parameters);
		public abstract Task ExecuteVoid(string procedureName);
		public abstract Task ExecuteVoid(string procedureName, DynamicParameters parameters);
	}
}
