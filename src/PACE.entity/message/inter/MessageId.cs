using System;
using System.Text;

namespace PACE.entity.message.inter
{
	public class MessageId
	{
		//private static int _index = 0;
		public string Domain { get; set; }
		public string IpAddress { get; set; }
		public long TimeStamp { get; set; }
		public int Index { get; set; }

		public MessageId(string domain, string ipAddress, long timeStamp, int index)
		{
			Domain = domain;
			IpAddress = ipAddress;
			TimeStamp = timeStamp;
			Index = index;
		}

		public static MessageId Parse(string messageId)
		{
			var id = new MessageId(null, null, 0, 0);
			if (string.IsNullOrEmpty(messageId)) return id;

			var parts = messageId.Split('-');
			var length = parts.Length;
			if (length >= 4)
			{
				id.IpAddress = parts[length - 3];
				id.TimeStamp = Convert.ToInt64(parts[length - 2]);
				id.Index = int.Parse(parts[length - 1]);
				if (length > 4)
				{
					var sb = new StringBuilder();
					for (int i = 0; i < length - 3; i++)
					{
						if (i > 0) sb.Append("-");
						sb.Append(parts[i]);
					}
					id.Domain = sb.ToString();
				}
				else
				{
					id.Domain = parts[0];
				}
			}
			return id;
		}

		public override string ToString()
		{
			return string.Format("{0}-{1}-{2}-{3}", Domain, IpAddress, TimeStamp, Index);
		}
	}
}
