using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using PACE.entity.message.inter;

namespace PACE.entity.message.spi
{
	public class DefaultMessageManager : IMessageManager
	{
		private const string StackKey = "_stack";
		private static int callCnt = 1;

		/// <summary>
		/// Only the transaction instance can be stacked
		/// </summary>
		private MessageStack _stack;

		/// <summary>
		/// Only the non-transaction instance queued, to be add as children message
		/// </summary>
		private readonly ConcurrentQueue<IMessage> _queue;

		public DefaultMessageManager()
		{
			_stack = CallContext.GetData(StackKey) as MessageStack;
			if (null == _stack) return;
			_queue = new ConcurrentQueue<IMessage>();
			var methodTrans = new DefaultTransaction(MessageType.Method, "Call Method.");
			_stack.Push(methodTrans);
			CallContext.SetData(StackKey, _stack);
		}


		/// <summary>
		/// add all the non-Itransaction message to the current transaction,
		/// push the current transaction into the stack,
		/// Complete method callee.
		/// </summary>
		public void Dispose()
		{
			_stack = CallContext.GetData(StackKey) as MessageStack;
			if (null == _stack || _stack.Count() < 1) return;
			ITransaction current = _stack.Pop();
			while (_queue.Count>0)
			{
				entity.message.IMessage msg;
				if (_queue.TryDequeue(out msg))
				{
					current.AddChild(msg);
				}
			}
			current.Complete();
			if (_stack.Count() > 0)
			{
				ITransaction parent = _stack.Pop();
				parent.AddChild(current);
				_stack.Push(parent);
			}
		}

		public bool Event(string msg)
		{
			DefaultEvent ev = new DefaultEvent(MessageType.Method, msg);
			ev.SetStatus(MessageStatus.Success);
			ev.Complete();
			_queue.Enqueue(ev);
			return true;
		}

		public bool Trace(string msg)
		{
			DefaultTrace trace = new DefaultTrace(MessageType.Method, msg);
			trace.SetStatus(MessageStatus.Success);
			trace.Complete();
			_queue.Enqueue(trace);
			return true;
		}
	}
}
