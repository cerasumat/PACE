namespace PACE.entity.message
{
	public interface IForkedTransaction : ITransaction
	{
		void Fork();
		string GetForkedMessageId();
	}
}
