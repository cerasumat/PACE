using System;
using System.Collections.Generic;
using System.Linq;
using PACE.entity.message.spi;
using PACE.Utility;

namespace PACE.entity.message.inter
{
	public class DefaultTransaction : AbstractMessage, ITransaction
	{
		private IMessageManager _manager;
		private IList<IMessage> _children;
		private bool _isRoot;
		private long _durationStart;
		private long _durationInMillis;

		public DefaultTransaction(MessageType type, string name) : this(type, name, null){}

		public DefaultTransaction(MessageType type, string name, IMessageManager manager) : base(type, name)
		{
			_manager = manager;
			_isRoot = false;
			_durationStart = DateTime.Now.ToMilliseconds();
			_durationInMillis = -1;
		}

		public ITransaction AddChild(IMessage message)
		{
			if (null == _children)
			{
				_children = new List<IMessage>();
			}
			if (null != message)
			{
				_children.Add(message);
			}
			else
			{
				//TODO: LogError
			}
			return this;
		}

		public IEnumerable<IMessage> GetChildren()
		{
			return _children ?? new List<IMessage>();
		}

		public long GetDurationInMillis()
		{
			if (_durationInMillis > 0)
			{
				return _durationInMillis;
			}
			else
			{
				long duration = 0;
				int len = null == _children ? 0 : _children.Count;
				if (len > 0)
				{
					IMessage lastChild = _children.Last();
					if (lastChild is ITransaction)
					{
						DefaultTransaction trans = (DefaultTransaction) lastChild;
						duration = trans.Timestamp.ToMilliseconds() - Timestamp.ToMilliseconds();
					}
					else
					{
						duration = lastChild.Timestamp.ToMilliseconds() - Timestamp.ToMilliseconds();
					}
				}
				return duration;
			}
		}

		public bool HasChildren()
		{
			return null != _children && _children.Count > 0;
		}

		public bool IsRoot()
		{
			return _isRoot;
		}

		public override void Complete()
		{
			try
			{
				if (IsComplete)
				{
					DefaultEvent ev = new DefaultEvent(MessageType.Pace, "Bad Operation");
					ev.SetStatus(MessageStatus.TransactionAlreadyComplete);
					ev.Complete();
					AddChild(ev);
				}
				else
				{
					_durationInMillis = DateTime.Now.ToMilliseconds() - _durationStart;
					SetComplete(true);
					//if (null != _manager)
					//{
					//	_manager.End(this);
					//}
				}
			}
			catch (Exception)
			{
				// TODO: ignore the Exception or handle this?
			}
		}

		public IMessageManager GetManager()
		{
			return _manager;
		}

		public void SetDurationInMillis(long duration)
		{
			_durationInMillis = duration;
		}

		public void SetRoot(bool isRoot)
		{
			_isRoot = isRoot;
		}

		public void SetDurationStart(long durationStart)
		{
			_durationStart = durationStart;
		}
	}
}
