using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace CRMBase.Services
{
	public sealed class DatabaseManager
	{
		public bool Exists(string dbName)
		{
			var query = $@"SELECT database_id FROM sys.databases WHERE Name = '{dbName}'";
			bool result = false;
			var connection = new SqlConnection(ConfigReader.Instance.Read("StartupConnectionString"));
			try
			{
				using (connection)
				{
					using (SqlCommand sqlCmd = new SqlCommand(query, connection))
					{
						connection.Open();
						object resultObj = sqlCmd.ExecuteScalar();
						int databaseID = 0;
						if (resultObj != null)
						{
							int.TryParse(resultObj.ToString(), out databaseID);
						}
						connection.Close();
						result = (databaseID > 0);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public void CreateIfNotExists(string sqlfilePath, string dbName = null)
		{
			if (dbName == null)
			{
				dbName = ConfigReader.Instance.Read("DatabaseName");
			}
			if (!Exists(dbName))
			{
				string script = File.ReadAllText(sqlfilePath);
				var commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
				using (var connection = new SqlConnection(ConfigReader.Instance.Read("StartupConnectionString")))
				{
					connection.Open();
					foreach (string commandString in commandStrings)
					{
						if (commandString.Trim() != "")
						{
							using (var command = new SqlCommand(commandString, connection))
							{
								command.ExecuteNonQuery();
							}
						}
					}
					connection.Close();
				}
			}
		}
	}
}