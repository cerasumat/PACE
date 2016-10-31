using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using PACE.client.Aop;
using PACE.entity.message;
using PACE.entity.message.inter;

namespace PACE.api.test.Models
{
	[Pace]
	public class TestBiz : ContextBoundObject
	{
		[PaceMethod]
		public string GetVersion()
		{
			var transaction = CallContext.GetData("_current") as DefaultTransaction;
			if (null != transaction)
			{
				DefaultEvent ev = new DefaultEvent(MessageType.Method, "Call Biz Method.");
				ev.SetStatus(MessageStatus.Success);
				ev.Complete();
				transaction.AddChild(ev);	
			}
			Thread.Sleep(new Random().Next(100));
			DoSomething();
			return "1.0.1";
		}

		[PaceMethod]
		private void DoSomething()
		{
			var transaction = CallContext.GetData("_current") as DefaultTransaction;
			if (null != transaction)
			{
				DefaultEvent ev = new DefaultEvent(MessageType.Method, "Call private Method.");
				ev.SetStatus(MessageStatus.Success);
				ev.Complete();
				transaction.AddChild(ev);
			}
			Thread.Sleep(new Random().Next(100));
		}
	}
}