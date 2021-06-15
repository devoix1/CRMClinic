namespace CRMBase.Messages
{
	public class VMUpdateMessage : IMessage {
		public VMUpdateMessage(object sender) {
			Sender = sender;
		}
		public object VM { get; set; }
		public object Sender { get; }
	}
}