using System;

namespace PACE.entity.message
{
	public interface IException : IMessage
	{
		Exception InnerException { get; set; }
	}
}
