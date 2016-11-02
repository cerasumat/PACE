using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

using PACE.client.Handler;

namespace PACE.client.Aop
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public class PaceAttribute : ContextAttribute, IContributeObjectSink
	{
		public PaceAttribute() : base("PaceLog"){}

		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
		{
			 return new DefaultMessageSink(nextSink);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class PaceMethodAttribute : Attribute{ }

}
