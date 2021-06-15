using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBase.Messages
{
	public class Message<T> : IMessage
	{
		public Message(object sender)
		{
			Sender = sender;
		}
		public object Sender { get; }
		public T Value { get; set; }
	}
}
