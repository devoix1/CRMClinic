using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public sealed class ProcedureManager : BaseProcedureManager
	{
		public override async Task<HashSet<T>> Execute<T>(string procedureName)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				return (await connection.QueryAsync<T>(procedureName, commandType: CommandType.StoredProcedure)).ToHashSet();
			}
		}
		public override async Task<HashSet<T>> Execute<T>(string procedureName, DynamicParameters parameters)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				return (await connection.QueryAsync<T>(procedureName, commandType: CommandType.StoredProcedure, param: parameters)).ToHashSet();
			}
		}
		public override async Task<T> ExecuteSingle<T>(string procedureName)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				return (await connection.QueryAsync<T>(procedureName, commandType: CommandType.StoredProcedure)).FirstOrDefault();
			}
		}
		public override async Task<T> ExecuteSingle<T>(string procedureName, DynamicParameters parameters)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				return (await connection.QueryAsync<T>(procedureName, commandType: CommandType.StoredProcedure, param: parameters)).FirstOrDefault();
			}
		}
		public override async Task ExecuteVoid(string procedureName)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);
			}
		}
		public override async Task ExecuteVoid(string procedureName, DynamicParameters parameters)
		{
			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure, param: parameters);
			}
		}
	}
}
