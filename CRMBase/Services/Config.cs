using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CRMBase.Services
{
	public sealed class Config : DynamicObject
	{
		public Config()
		{
			members = new Dictionary<string, object>();
		}
		private readonly Dictionary<string, object> members;
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (members.ContainsKey(binder.Name))
			{
				result = members[binder.Name];
				return true;
			}
			result = null;
			return false;
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			members[binder.Name] = value;
			return true;
		}
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			try
			{
				if (members.ContainsKey(indexes[0] as string))
				{
					result = members[indexes[0] as string];
					return true;
				}

				return false;
			}
			catch (Exception ex) when (ex is IndexOutOfRangeException || ex is InvalidCastException)
			{
				return false;
			}
		}
	}
}
