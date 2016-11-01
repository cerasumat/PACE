using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
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
				mng.Trace("Call 3rd Method.");
				Thread.Sleep(new Random().Next(100));
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