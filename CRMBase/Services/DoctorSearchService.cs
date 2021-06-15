using CRMBase.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CRMBase.Services
{
	public sealed class DoctorSearchService : ISearchService<Doctor, string>
	{
		public HashSet<Doctor> Search(HashSet<Doctor> inputValues, string searchValue)
		{
			string GetSeachPattern(Doctor doctor)
			{
				return $"{doctor.Name.ToLower()} {doctor.Surname.ToLower()} {doctor.Middlename.ToLower()} {doctor.Phone} {doctor.Email.ToLower()}";
			}
			var result = new HashSet<Doctor>();
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
