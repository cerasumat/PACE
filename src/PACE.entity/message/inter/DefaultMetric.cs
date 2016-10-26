using PACE.entity.message.spi;

namespace PACE.entity.message.inter
{
	public class DefaultMetric : AbstractMessage, IMetric
	{
		private IMessageManager _manager;

		public DefaultMetric(MessageType type, string name) : base(type, name){}

		public DefaultMetric(MessageType type, string name, IMessageManager manager) : base(type, name)
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
