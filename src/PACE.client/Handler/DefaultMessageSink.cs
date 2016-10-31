using System;
using System.Runtime.Remoting.Messaging;
using PACE.client.Aop;
using PACE.entity.message;
using PACE.entity.message.inter;
using IMessage = System.Runtime.Remoting.Messaging.IMessage;

namespace PACE.client.Handler
{
	public class DefaultMessageSink	: AbstractMessageSink
	{
		private const string ParentKey = "_parent";
		private const string ChildKey = "_current";

		public DefaultMessageSink(IMessageSink nextSink) : base(nextSink){ }

		public override IMessage SyncProcessMessage(IMessage msg)
		{
			IMessage rtnMessage = null;
			IMethodCallMessage call = msg as IMethodCallMessage;
			if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(PaceMethodAttribute))) == null)
			{
				rtnMessage = NextSink.SyncProcessMessage(msg);
			}
			else
			{
				var parentTrans = CallContext.GetData(ParentKey) as DefaultTransaction;
				var childTrans = new DefaultTransaction(MessageType.Method, "Call Method");
				var beginEvent = new DefaultEvent(MessageType.Event, "Method Begin");
				beginEvent.SetStatus(MessageStatus.Success);
				beginEvent.Complete();
				childTrans.AddChild(beginEvent);
				CallContext.SetData(null == parentTrans ? ParentKey : ChildKey, childTrans);
				//Call method sync
				rtnMessage = NextSink.SyncProcessMessage(msg);
				childTrans = CallContext.GetData(null == parentTrans ? ParentKey : ChildKey) as DefaultTransaction;
				if (null != childTrans)
				{
					var endEvent = new DefaultEvent(MessageType.Event, "Method End");
					endEvent.SetStatus(MessageStatus.Success);
					endEvent.Complete();
					childTrans.AddChild(endEvent);
					childTrans.Complete();
					if (null != parentTrans)
					{
						parentTrans.AddChild(childTrans);
					}
					else
					{
						childTrans.SetRoot(true);
						CallContext.SetData(ParentKey, childTrans);
					}
				}
			}
			return rtnMessage;
		} 
	}
}
