using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PACE.entity.message;
using PACE.entity.message.inter;

namespace PACE.client.Aop
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class PaceFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
	{
		private const string ParentKey = "_parent";

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (SkipLogging(actionContext))
			{
				return;
			}
			var transaction = new DefaultTransaction(MessageType.Url, actionContext.Request.RequestUri.AbsolutePath);
			transaction.SetRoot(true);
			CallContext.SetData(ParentKey, transaction);
			//var stopWatch = new Stopwatch();
			//actionContext.Request.Properties[Key] = stopWatch;
			//stopWatch.Start(); 
		}

		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			//if (!actionExecutedContext.Request.Properties.ContainsKey(Key))
			//{
			//	return;
			//}

			//var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
			//if (stopWatch != null)
			//{
			//	stopWatch.Stop();
			//	var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
			//	var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			//	var msg = string.Format("[Execution of {0}.{1} took {2}.]", controllerName, actionName, stopWatch.Elapsed);
			//	actionExecutedContext.Response.Headers.Add("Duration", msg);
				
			//	var param = CallContext.GetData("_root");
			//	// TODO: MessageManager handle the _root infomation 
			//}

			if (null == CallContext.GetData(ParentKey))
			{
				return;
			}
			ITransaction transaction = CallContext.GetData(ParentKey) as DefaultTransaction;
			if (null != transaction)
			{
				transaction.Complete();
				var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
				var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
				var msg = string.Format("[Execution of {0}.{1} took {2} millis.]", controllerName, actionName,
					transaction.GetDurationInMillis());
				actionExecutedContext.Response.Headers.Add("Duration", msg);
			}
			
		}

		private static bool SkipLogging(HttpActionContext actionContext)
		{
			return actionContext.ActionDescriptor.GetCustomAttributes<PaceIgnoreAttribute>().Any() ||
					actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<PaceIgnoreAttribute>().Any();
		}
	}

	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class PaceIgnoreAttribute : Attribute { } 
}
