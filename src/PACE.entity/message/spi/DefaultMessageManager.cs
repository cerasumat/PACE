using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
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

		public DefaultMessageManager(params object[] args)
		{
			_stack = CallContext.LogicalGetData(StackKey) as MessageStack;
			if (null == _stack) return;
			_queue = new ConcurrentQueue<IMessage>();
			var methodTrans = new DefaultTransaction(MessageType.Method, (new StackFrame(1, false)).GetMethod().Name);
			methodTrans.Parameters = args;
			_stack.Push(methodTrans);
			CallContext.LogicalSetData(StackKey, _stack);
		}


		/// <summary>
		/// add all the non-Itransaction message to the current transaction,
		/// push the current transaction into the stack,
		/// Complete method callee.
		/// </summary>
		public void Dispose()
		{
			_stack = CallContext.LogicalGetData(StackKey) as MessageStack;
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

		public bool Event<T>(T ev) where T : AbstractMessage, IEvent
		{
			if (null == ev) return false;
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

		public bool Trace<T>(T trace) where T : AbstractMessage, ITrace
		{
			if (null == trace) return false;
			trace.SetStatus(MessageStatus.Success);
			trace.Complete();
			_queue.Enqueue(trace);
			return true;
		}

		public bool Error(Exception exp, [CallerMemberName] string caller = "",
			 [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
		{
			DefaultException e = new DefaultException(MessageType.Error, exp.Message, exp);
			e.SetStatus(MessageStatus.Success);
			e.CallerName = caller;
			e.FilePath = path;
			e.LineNum = line;
			e.Complete();
			_queue.Enqueue(e);
			return true;
		}

		public bool Error<T>(T exp, [CallerMemberName] string caller = "", [CallerFilePath] string path = "",
			[CallerLineNumber] int line = 0) where T : AbstractMessage, IException
		{
			exp.SetStatus(MessageStatus.Success);
			exp.CallerName = caller;
			exp.FilePath = path;
			exp.LineNum = line;
			exp.Complete();
			_queue.Enqueue(exp);
			return true;
		}

		public bool Error(string errorMessage, [CallerMemberName] string caller = "",
			 [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
		{
			DefaultException e = new DefaultException(MessageType.Error, errorMessage, errorMessage);
			e.SetStatus(MessageStatus.Success);
			e.CallerName = caller;
			e.FilePath = path;
			e.LineNum = line;
			e.Complete();
			_queue.Enqueue(e);
			return true;
		}
	}
}
