using System;
using System.Collections.Generic;
using System.Linq;

namespace PACE.entity.message.inter
{
	public class AbstractMessage : IMessage
	{
		public AbstractMessage(MessageType type, string name)
		{
			Type = type;
			Name = name;
			Timestamp = DateTime.Now;
			Status = MessageStatus.Success.ToString();
			IsComplete = false;
		}

		public bool IsSuccess { get { return Status.Equals(MessageStatus.Success.ToString()); } }
		public bool IsComplete { get; private set; }
		public IDictionary<string, string> Data { get; private set; }
		public string Name { get; private set; }
		public string Status { get; private set; }
		public MessageType Type { get; private set; }
		public DateTime Timestamp { get; private set; }
		public void AddData(string keyValuePairs)
		{
			if (null == Data)
			{
				Data = new Dictionary<string, string>();
			}
			if (string.IsNullOrEmpty(keyValuePairs)) return;
			var kvs = keyValuePairs.Split('&');
			if (!kvs.Any()) return;
			foreach (var kv in kvs)
			{
				var pair = kv.Split('=');
				if (pair.Length == 2)
				{
					Data.Add(pair[0], pair[1]);
				}
			}
		}

		public void AddData(string key, string value)
		{
			if (null == Data)
			{
				Data = new Dictionary<string, string>();
			}
			if (string.IsNullOrEmpty(key)) return;
			Data.Add(key, value);
		}

		public void SetStatus(string status)
		{
			Status = status;
		}

		public void SetStatus(Exception e)
		{
			Status = e.GetType().Name;
		}

		public void SetComplete(bool isComplete)
		{
			IsComplete = isComplete;
		}

		public virtual void Complete()
		{
		}

		public override string ToString()
		{
			return string.Format("Message:{0}-{1}-{2}:{3}", Name, Type, Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), IsComplete);
		}
	}
}
