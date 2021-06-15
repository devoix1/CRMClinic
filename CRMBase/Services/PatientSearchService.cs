using CRMBase.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CRMBase.Services
{
	public sealed class PatientSearchService : ISearchService<Patient, string>
	{
		public HashSet<Patient> Search(HashSet<Patient> inputValues, string searchValue)
		{
			string GetSeachPattern(Patient doctor)
			{
				return $"{doctor.Name.ToLower()} {doctor.Surname.ToLower()} {doctor.Middlename.ToLower()} {doctor.Phone} {doctor.Email.ToLower()}";
			}
			var regex = new Regex(@"");
			var result = new HashSet<Patient>();
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
