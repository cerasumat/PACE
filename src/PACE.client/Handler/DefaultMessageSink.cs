using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using PACE.client.Aop;
using PACE.entity.message;
using PACE.entity.message.inter;
using PACE.entity.message.spi;
using IMessage = System.Runtime.Remoting.Messaging.IMessage;

namespace PACE.client.Handler
{
	public class DefaultMessageSink	: AbstractMessageSink
	{
		//private const string ParentKey = "_parent";
		//private const string ChildKey = "_current";

		private const string StackKey = "_current";

		public DefaultMessageSink(IMessageSink nextSink) : base(nextSink){ }

		//public override IMessage SyncProcessMessage(IMessage msg)
		//{
		//	IMessage rtnMessage = null;
		//	IMethodCallMessage call = msg as IMethodCallMessage;
		//	if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(PaceMethodAttribute))) == null)
		//	{
		//		rtnMessage = NextSink.SyncProcessMessage(msg);
		//	}
		//	else
		//	{
		//		var parentTrans = CallContext.GetData(ParentKey) as DefaultTransaction;
		//		var childTrans = new DefaultTransaction(MessageType.Method, "Call Method");
		//		var beginEvent = new DefaultEvent(MessageType.Event, "Method Begin");
		//		beginEvent.SetStatus(MessageStatus.Success);
		//		beginEvent.Complete();
		//		childTrans.AddChild(beginEvent);
		//		CallContext.SetData(null == parentTrans ? ParentKey : ChildKey, childTrans);
		//		//Call method sync
		//		rtnMessage = NextSink.SyncProcessMessage(msg);
		//		childTrans = CallContext.GetData(null == parentTrans ? ParentKey : ChildKey) as DefaultTransaction;
		//		if (null != childTrans)
		//		{
		//			var endEvent = new DefaultEvent(MessageType.Event, "Method End");
		//			endEvent.SetStatus(MessageStatus.Success);
		//			endEvent.Complete();
		//			childTrans.AddChild(endEvent);
		//			childTrans.Complete();
		//			if (null != parentTrans)
		//			{
		//				parentTrans.AddChild(childTrans);
		//			}
		//			else
		//			{
		//				childTrans.SetRoot(true);
		//				CallContext.SetData(ParentKey, childTrans);
		//			}
		//		}
		//	}
		//	return rtnMessage;
		//} 

		public override IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage rtnMessage = null;
			var call = msg as IMethodCallMessage;
			if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof (PaceMethodAttribute))) == null)
			{
				rtnMessage = NextSink.SyncProcessMessage(msg);
			}
			else
			{
				// Method executing
				var stack = CallContext.GetData(StackKey) as MessageStack;
				if (null == stack) stack = new MessageStack();
				var methodTrans = new DefaultTransaction(MessageType.Method, "Call Method.");
				stack.Push(methodTrans);
				CallContext.SetData(StackKey, stack);
				// Call method sync
				rtnMessage = NextSink.SyncProcessMessage(msg);
				// Method executed
				// 消息出栈处理，构建消息链式消息容器结构
				stack = CallContext.GetData(StackKey) as MessageStack;
				if (null == stack) return rtnMessage;

				var childEvents = new List<entity.message.IMessage>();
				var childTrans = new List<entity.message.IMessage>();
				while (!stack.IsEmpty())
				{
					entity.message.IMessage current = stack.Peek();
					if (current.Id.Equals(methodTrans.Id)) break;
					current = stack.Pop();
					var currentTran = current as ITransaction;
					if (null != currentTran)
					{
						if (childEvents.Count > 0)
						{
							foreach (var child in childEvents)
							{
								currentTran.AddChild(child);
							}
						}
						currentTran.Complete();
						childTrans.Add(currentTran);
					}
					else
					{
						childEvents.Add(current);
					}
				}
				var currentTrans = stack.Pop() as ITransaction;
				if (null == currentTrans) return rtnMessage;
				foreach (var childTran in childTrans)
				{
					currentTrans.AddChild(childTran);
				}
				foreach (var childEvent in childEvents)
				{
					currentTrans.AddChild(childEvent);
				}
				currentTrans.Complete();
				stack.Push(currentTrans);
				CallContext.SetData(StackKey, stack);
			}
			return rtnMessage;
		}
	}
}
