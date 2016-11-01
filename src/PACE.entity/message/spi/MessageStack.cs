using System;
using System.Collections.Concurrent;

namespace PACE.entity.message.spi
{
	public class MessageStack
	{
		private readonly ConcurrentStack<IMessage> _messageStack;

		public MessageStack()
		{
			_messageStack = new ConcurrentStack<IMessage>();
		}

		public bool Push(IMessage message)
		{
			try
			{
				_messageStack.Push(message);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public IMessage Pop()
		{
			IMessage message;
			_messageStack.TryPop(out message);
			return message;
		}

		public IMessage Peek()
		{
			IMessage message;
			_messageStack.TryPeek(out message);
			return message;
		}

		public int Count()
		{
			return _messageStack.Count;
		}

		public bool Clear()
		{
			try
			{
				_messageStack.Clear();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool IsEmpty()
		{
			return _messageStack.IsEmpty;
		}

	}
}
