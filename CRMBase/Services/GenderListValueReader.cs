using Newtonsoft.Json;
using System.Collections.Generic;

namespace CRMBase.Services
{
	public sealed class GenderListValueReader : IConfigValueReader<List<string>>
	{
		public List<string> Parse(object value)
		{
			try
			{
				return JsonConvert.DeserializeObject<List<string>>(value.ToString());
			}
			catch
			{
				return null;
			}
		}
	}
}
