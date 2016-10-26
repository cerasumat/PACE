using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PACE.entity.message.spi;

namespace PACE.entity.message.inter
{
	public class DefaultForkedTransaction : DefaultTransaction, IForkedTransaction
	{
		private string _rootMessageId;
		private string _parentMessageId;
		private string _forkedMessageId;

		public DefaultForkedTransaction(MessageType type, string name) : base(type, name){ }

		public DefaultForkedTransaction(MessageType type, string name, IMessageManager manager) : base(type, name, manager)
		{
			SetRoot(false);
			IMessageTree tree = manager.GetThreadLocalMessageTree();
			if (null != tree)
			{
				_rootMessageId = tree.RootMessageId;
				_parentMessageId = tree.MessageId;
				_forkedMessageId = 
			}
		}

		public void Fork()
		{
			throw new NotImplementedException();
		}

		public string GetForkedMessageId()
		{
			throw new NotImplementedException();
		}
	}
}
