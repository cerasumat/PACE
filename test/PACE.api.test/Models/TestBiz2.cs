using System;
using System.Threading;
using PACE.client.Aop;
using PACE.entity.message;
using PACE.entity.message.inter;
using PACE.entity.message.spi;

namespace PACE.api.test.Models
{
	public class TestBiz2 : ContextBoundObject
	{
		[PaceMethod]
		public void DoSomething2()
		{
			using (var mng = new DefaultMessageManager())
			{
				mng.Trace("Call 3rd Method Step 1.");
				var trace = new DefaultTrace(MessageType.Method, "Call 3rd Method Step 2.");
				mng.Trace<DefaultTrace>(trace);
				Thread.Sleep(new Random().Next(100));
				DoSomething3();
			}
		}

		[PaceMethod]
		public void DoSomething3()
		{
			using (var mng = new DefaultMessageManager())
			{
				mng.Trace("Call 4th Method.");
				Thread.Sleep(new Random().Next(100));
			}
		}
	}
}