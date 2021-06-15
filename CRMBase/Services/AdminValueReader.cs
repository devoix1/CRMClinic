using CRMBase.Model;
using Newtonsoft.Json;

namespace CRMBase.Services
{
	public sealed class AdminValueReader : IConfigValueReader<Admin>
	{
		public Admin Parse(object value)
		{
				return JsonConvert.DeserializeObject<Admin>(value.ToString());
			//try
			//{
			//}
			//catch
			//{
			//	return null;
			//}
		}
	}
}
