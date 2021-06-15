using CRMBase.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CRMBase.Services
{
	public sealed class AdminSearchService : ISearchService<Admin, string>
	{
		public HashSet<Admin> Search(HashSet<Admin> inputValues, string searchValue)
		{
			string GetSeachPattern(Admin admin)
			{
				return $"{admin.Name.ToLower()} {admin.Surname.ToLower()} {admin.Middlename.ToLower()} {admin.Phone} {admin.Email.ToLower()}";
			}
			var regex = new Regex(@"");
			var result = new HashSet<Admin>();
			foreach (var item in inputValues)
			{
				if (GetSeachPattern(item).Contains(searchValue))
				{
					result.Add(item);
				}
			}
			return result;
		}
	}
}
