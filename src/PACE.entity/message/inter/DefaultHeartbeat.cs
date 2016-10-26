using PACE.entity.message.spi;

namespace PACE.entity.message.inter
{
	public class DefaultHeartbeat : AbstractMessage, IEvent
	{
		private IMessageManager _manager;

		public DefaultHeartbeat(MessageType type, string name) : base(type, name){ }

		public DefaultHeartbeat(MessageType type, string name, IMessageManager manager) : base(type, name)
		{
			_manager = manager;
		}

		public override void Complete()
		{
			SetComplete(true);
			if (null != _manager)
			{
				_manager.Add(this);
			}
		}
	}
}
