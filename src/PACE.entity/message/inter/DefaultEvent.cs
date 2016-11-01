using PACE.entity.message.spi;

namespace PACE.entity.message.inter
{
	public class DefaultEvent : AbstractMessage, IEvent
	{
		private IMessageManager _manager;

		public DefaultEvent(MessageType type, string name) : base(type, name){ }

		public DefaultEvent(MessageType type, string name, IMessageManager manager) : base(type, name)
		{
			_manager = manager;
		}

		public override void Complete()
		{
			SetComplete(true);
			//if (null != _manager)
			//{
			//	_manager.Add(this);
			//}
		}
	}
}
