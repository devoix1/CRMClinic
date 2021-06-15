using System.Collections.Generic;

namespace CRMBase.Model
{
	// НЕ является таблицей. PRIMARY KEY Id для составляющих таблиц
	public abstract class HashComparator
	{
		public int Id { get; set; }
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (obj is HashComparator o)
			{
				return o.GetHashCode() == this.GetHashCode();
			}
			return false;
		}
	}
}
