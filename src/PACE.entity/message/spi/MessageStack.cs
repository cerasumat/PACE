using System;
using System.Collections.Concurrent;

namespace PACE.entity.message.spi
{
	public class MessageStack
	{
		private readonly ConcurrentStack<ITransaction> _messageStack;

		public MessageStack()
		{
			_messageStack = new ConcurrentStack<ITransaction>();
		}

		public bool Push(ITransaction message)
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

		public ITransaction Pop()
		{
			ITransaction message;
			_messageStack.TryPop(out message);
			return message;
		}

		public ITransaction Peek()
		{
			ITransaction message;
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
