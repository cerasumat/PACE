using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PACE.Utility;

namespace PACE.entity.message.inter
{
	public class MessageIdFactory
	{
		private static int _index = 0;
		public string Domain { get; private set; }
		public static string IpAddress { get; private set; }

		public MessageIdFactory() : this("Default") { }

		public MessageIdFactory(string domain)
		{
			Domain = domain;
			if (string.IsNullOrEmpty(IpAddress))
				IpAddress = NetworkInterfaceManager.GetIp();
		}

		public MessageId GetNextId(string messageId)
		{
			return MessageId.Parse(messageId);
		}

		/// <summary>
		/// Timestamp values seconds, could be used to count the messages per second(MPS)
		/// </summary>
		/// <returns></returns>
		public MessageId GetNextId()
		{
			return new MessageId(Domain, IpAddress, DateTime.Now.ToMilliseconds()/1000, NextIndex());
		}

		private int NextIndex()
		{
			return Math.Abs(Interlocked.Increment(ref _index));
		}

	}
}
