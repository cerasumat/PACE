using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PACE.entity.message.inter
{
	public class MessageId
	{
		private static int _version = 0;
		public string Domain { get; set; }
		public string IpAddress { get; set; }
		public long TimeStamp { get; set; }

		public MessageId(string domain, string ipAddress, long timeStamp)
		{
			
		}

		private int GetCurrentVersion()
		{
			return Interlocked.Increment(ref _version);
		}
	}
}
