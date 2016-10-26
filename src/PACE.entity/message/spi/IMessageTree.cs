using System;

namespace PACE.entity.message.spi
{
	public interface IMessageTree : ICloneable
	{
		IMessageTree Copy();
		string Domain { get; set; }
		string HostName { get; set; }
		string IpAddress { get; set; }
		IMessage Message { get; set; }
		string MessageId { get; set; }
		string ParentMessageId { get; set; }
		string RootMessageId { get; set; }
		string SessionToken { get; set; }
		string ThreadGroupName { get; set; }
		string ThreadId { get; set; }
		string ThreadName { get; set; }
		/// <summary>
		/// 测试消息树
		/// </summary>
		bool IsSample { get; set; }
	}
}
