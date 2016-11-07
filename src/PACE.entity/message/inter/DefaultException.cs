using System;
using System.Runtime.CompilerServices;

namespace PACE.entity.message.inter
{
	public class DefaultException : AbstractMessage, IException
	{
		public DefaultException(MessageType type, string name, Exception exp) : base(type, name)
		{
			InnerException = exp;
		}

		public DefaultException(MessageType type, string name, string exp)
			: base(type, name)
		{
			InnerException = new Exception(exp);
		}

		public override void Complete()
		{
			SetComplete(true);
		}

		public Exception InnerException { get; set; }
		public string CallerName { get; set; }
		public string FilePath { get; set; }
		public int LineNum { get; set; }
	}
}
