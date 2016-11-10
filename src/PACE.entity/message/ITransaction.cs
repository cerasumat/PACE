using System.Collections.Generic;

namespace PACE.entity.message
{
	/// <summary>
	/// <code>ITransaction</code> is any interesting unit of work that takes time to complete and may fail.
	/// 
	/// Basically, all data access across the boundary needs to be logged as a <code>Transaction</code> since it may fail and time consuming. 
	/// For example, URL request, disk IO, DB query, search query, HTTP request, 3rd party API call etc.
	/// 
	/// Sometime if A needs call B which is owned by another team, although A and B are deployed together without any physical boundary.
	/// To make the ownership clear, there could be some <code>ITransaction</code> logged when A calls B.
	/// 
	/// Most of <code>ITransaction</code> should be logged in the infrastructure level or framework level, 
	/// which is transparent to the application.
	/// 
	/// All message will be constructed as a message tree and send to back-end for further analysis, and for monitoring.
	/// Only <code>ITransaction</code> can be a tree node, all other message will be the tree leaf.
	/// The transaction without	other messages nested is an atomic transaction.
	/// </summary>
	public interface ITransaction : IMessage
	{
		IEnumerable<IMessage> Children { get; }

		/// <summary>
		/// Add one nested child message to current transaction.
		/// </summary>
		/// <param name="message">message to be added</param>
		/// <returns>transaction it self</returns>
		ITransaction AddChild(IMessage message);

		/// <summary>
		/// Get all children message within current transaction.
		/// Typically, a <code>ITransaction</code> can nest other <code>ITransaction</code>s, <code>IEvent</code>s and
		/// <code>IHeartbeat</code> s, while an <code>IEvent</code> or <code>IHeartbeat</code> can't nest other messages.
		/// </summary>
		/// <returns>all children messages, empty if there is no nested children.</returns>
		IEnumerable<IMessage> GetChildren();

		/// <summary>
		/// How long the transaction took from construction to complete. Time unit is millisecond.
		/// </summary>
		/// <returns>duration time in millisecond</returns>
		long GetDurationInMillis();

		long DurationInMillis { get; }

		long DurationStart { get; }

		/// <summary>
		/// Has children or not. An atomic transaction does not have any children message.
		/// </summary>
		/// <returns>true if child exists, else false.</returns>
		bool HasChildren();

		/// <summary>
		/// Check if the transaction is root-transaction or belongs to another one.
		/// </summary>
		/// <returns>true if it's an root transaction.</returns>
		bool IsRoot();

		bool Root { get; }

		/// <summary>
		/// Get value for the root transaction
		/// New value from the begining of the http request chain,
		/// same value from the internal call of the biz.
		/// </summary>
		/// <returns></returns>
		//string HttpRequestId();

		/// <summary>
		/// Indicate the http request count in a http transaction
		/// </summary>
		/// <returns></returns>
		//int HttpRequestIndex();
	}
}
