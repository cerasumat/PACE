namespace PACE.entity.message
{
	public interface ITaggedTransaction	: ITransaction
	{
		void Bind(string tag, string childMessageId, string title);
		string GetParentMessageId();
		string GetRootMessageId();
		string GetTag();
		void Start();
	}
}
