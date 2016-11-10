using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using PACE.entity.message;
using PACE.entity.message.inter;
using PACE.entity.message.spi;

namespace PACE.client.Aop
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class PaceFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
	{
		//private const string ParentKey = "_parent";

		private const string StackKey = "_stack";
		private MessageId _currentId;

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (SkipLogging(actionContext))
			{
				return;
			}

			var stack = new MessageStack();
			var transaction = new DefaultTransaction(MessageType.Url, actionContext.Request.RequestUri.AbsolutePath);
			transaction.SetRoot(true);
			_currentId = transaction.Id;
			stack.Push(transaction);
			CallContext.LogicalSetData(StackKey, stack);
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

			MessageStack stack = CallContext.LogicalGetData(StackKey) as MessageStack;
			if(null == stack) return;
			var subMessages = new List<entity.message.IMessage>();
			while (stack.Count() > 1)
			{
				subMessages.Add(stack.Pop());
			}
			ITransaction rootMessage = stack.Pop() as ITransaction;
			if (null == rootMessage || (!rootMessage.Id.Equals(_currentId))) return;
			foreach (var msg in subMessages)
			{
				rootMessage.AddChild(msg);
			}
			rootMessage.Complete();

			//var json = JsonConvert.SerializeObject(rootMessage);
			//TODO: IO
			var newId = new MessageIdFactory().GetNextId();

			var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
			var controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			var duration = string.Format("[Execution of {0}.{1} took {2} millis.]", controllerName, actionName,
				rootMessage.GetDurationInMillis());
			actionExecutedContext.Response.Headers.Add("Duration", duration);
			
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
