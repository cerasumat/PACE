using System;

namespace PACE.entity.message
{
	public interface IException : IMessage
	{
		Exception InnerException { get; set; }

		string CallerName { get; set; }
		string FilePath { get; set; }
		int LineNum { get; set; }
	}
}
