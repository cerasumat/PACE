using System.Runtime.Remoting.Messaging;

namespace PACE.client.Handler
{
	public abstract class AbstractMessageSink : IMessageSink
	{
		protected AbstractMessageSink(IMessageSink nextSink)
		{
			NextSink = nextSink;
		}

		public abstract IMessage SyncProcessMessage(IMessage msg);

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return null;
		}

		public IMessageSink NextSink { get; private set; }
	}
}
