using System;
using System.Collections.Generic;

namespace CRMBase.Services
{
	public interface ISearchService<T, Y>
	{
		HashSet<T> Search(HashSet<T> inputValues, Y searchValue);
	}
}