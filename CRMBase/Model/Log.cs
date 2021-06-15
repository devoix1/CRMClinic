using System;

namespace CRMBase.Model
{
	public class Log : HashComparator
	{
		public Log()
		{
			Timestamp = DateTime.Now;
		}
		public string Message { get; set; }
		public ActionType ActionType { get; set; }
		public LogType LogType { get; set; }
		public DateTime? Timestamp { get; set; }
	}
}
