using CRMBase.Model;
using System.Collections.Generic;

namespace CRMBase.Services
{
	public sealed class RecordSearchService : ISearchService<Record, string>
	{
		public HashSet<Record> Search(HashSet<Record> inputValues, string searchValue)
		{
			string GetSeachPattern(Record record)
			{
				return $"{record.Patient.Name} {record.Patient.Surname.ToLower()} {record.Patient.Middlename.ToLower()}";
			}
			var result = new HashSet<Record>();
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
