using PACE.entity.message.spi;

namespace PACE.entity.message.inter
{
	public class DefaultTrace : AbstractMessage, ITrace
	{
		private IMessageManager _manager;
		public DefaultTrace(MessageType type, string name) : base(type, name){}

		public DefaultTrace(MessageType type, string name, IMessageManager manager) : base(type, name)
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
