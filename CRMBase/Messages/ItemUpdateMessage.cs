namespace CRMBase.Messages
{
	public class ItemUpdateMessage<T> : IMessage {
		public ItemUpdateMessage(object sender) {
			Sender = sender;
		}
		public object Sender { get; }
		public T Item { get; set; }
	}
}
