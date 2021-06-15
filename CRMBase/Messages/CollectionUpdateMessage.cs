using System.Collections.Generic;

namespace CRMBase.Messages
{
	public class CollectionUpdateMessage<T> : IMessage where T : class {
		public CollectionUpdateMessage(object sender) {
			Sender = sender;
		}
		public HashSet<T> Collection { get; set; }
		public object Sender { get; }
	}
}
