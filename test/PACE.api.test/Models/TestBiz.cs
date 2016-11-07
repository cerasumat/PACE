using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using PACE.client.Aop;
using PACE.entity.message;
using PACE.entity.message.inter;
using PACE.entity.message.spi;

namespace PACE.api.test.Models
{
	[Synchronization]
	public class TestBiz : ContextBoundObject
	{
		public string GetVersion()
		{
			//var stack = CallContext.GetData("_current") as MessageStack;
			//if (null != stack)
			//{
			//	DefaultEvent ev = new DefaultEvent(MessageType.Method, "Call 1st Method.");
			//	ev.SetStatus(MessageStatus.Success);
			//	ev.Complete();
			//	stack.Push(ev);	
			//}
			//Thread.Sleep(new Random().Next(100));
			//DoSomething();
			//var biz2 = new TestBiz2();
			//biz2.DoSomething2();
			//biz2.DoSomething3();
			//return "1.0.1";

			using (var mng = new DefaultMessageManager())
			{
				mng.Event("Call 1st Method.");
				Thread.Sleep(new Random().Next(100));
				DoSomething("XXXXXX");
				var biz2 = new TestBiz2();
				biz2.DoSomething2();
				return "1.0.1";
			}
		}

		private void DoSomething(string msg)
		{
			//var stack = CallContext.GetData("_current") as MessageStack;
			//if (null != stack)
			//{
			//	DefaultEvent ev = new DefaultEvent(MessageType.Method, "Call 2nd Method.");
			//	ev.SetStatus(MessageStatus.Success);
			//	ev.Complete();
			//	stack.Push(ev);
			//}
			//Thread.Sleep(new Random().Next(100));
			using (var mng = new DefaultMessageManager(msg))
			{
				try
				{
					throw new ArgumentException("exception inner 2nd method.");
				}
				catch (Exception exp)
				{
					mng.Error(exp);
					return;
				}
				mng.Trace("Call 2nd Method.");
				Thread.Sleep(new Random().Next(100));
			}
		}
	}
}