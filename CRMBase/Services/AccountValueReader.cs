using CRMBase.Model;
using Newtonsoft.Json;

namespace CRMBase.Services
{
	public sealed class AccountValueReader : IConfigValueReader<Account>
	{
		public Account Parse(object value)
		{
			try
			{
				return JsonConvert.DeserializeObject<Account>(value.ToString());
			}
			catch
			{
				return null;
			}
		}
	}
}
