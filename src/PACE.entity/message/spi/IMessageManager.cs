using System;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace PACE.entity.message.spi
{
	//public interface IMessageManager 
	//{
	//	void Add(IMessage message);

	//	/// <summary>
	//	/// Be triggered when a transaction ends, whatever it's the root transaction or nested transaction.
	//	/// However, if it's the root transaction then it will be flushed to back-end server asynchronously.
	//	/// </summary>
	//	/// <param name="transaction">transaction that need to be ended.</param>
	//	void End(ITransaction transaction);

	//	/// <summary>
	//	/// Get peek transaction for current thread.
	//	/// </summary>
	//	/// <returns>peek transaction for current thread, null if no transaction there.</returns>
	//	ITransaction GetPeekTransaction();

	//	/// <summary>
	//	/// Get thread local message information.
	//	/// </summary>
	//	/// <returns>message tree, null means current thread is not setup correctly.</returns>
	//	IMessageTree GetThreadLocalMessageTree();

	//	/// <summary>
	//	/// Check if the thread context is setup or not.
	//	/// </summary>
	//	/// <returns>true if the thread context is setup, false otherwise.</returns>
	//	bool HasContext();

	//	/// <summary>
	//	/// Check if current context logging is enabled or disabled.
	//	/// </summary>
	//	/// <returns>true if current context is enabled</returns>
	//	bool IsMessageEnable();

	//	/// <summary>
	//	/// Check if PACE logging is enabled or disabled.
	//	/// </summary>
	//	/// <returns>true if PACE is enabled</returns>
	//	bool IsPaceEnable();

	//	/// <summary>
	//	/// Check if PACE trace mode is enabled or disabled.
	//	/// </summary>
	//	/// <returns>true if PACE is trace mode</returns>
	//	bool IsTraceMode();

	//	/// <summary>
	//	/// Do cleanup for current thread environment in order to release resources in thread local objects.
	//	/// </summary>
	//	void Reset();

	//	/// <summary>
	//	/// Set PACE trace mode.
	//	/// </summary>
	//	/// <param name="traceMode"></param>
	//	void SetTraceMode(bool traceMode);

	//	/// <summary>
	//	/// Do setup for current thread environment in order to prepare thread local objects.
	//	/// </summary>
	//	void Setup();

	//	/// <summary>
	//	/// Be triggered when a new transaction starts, whatever it's the root transaction or nested transaction.
	//	/// </summary>
	//	/// <param name="transaction"></param>
	//	/// <param name="forked"></param>
	//	void Start(ITransaction transaction, bool forked);

	//	/// <summary>
	//	/// Binds the current message tree to the transaction tagged with <code>tag</code>.
	//	/// </summary>
	//	/// <param name="tag">tag name of the tagged transaction</param>
	//	/// <param name="title">title shown in the logview</param>
	//	void Bind(String tag, String title);

	//	/// <summary>
	//	/// Get the domain name.
	//	/// </summary>
	//	/// <returns></returns>
	//	string GetDomain();
	//}

	public interface IMessageManager : IDisposable
	{
		bool Event(string msg);
		bool Trace(string msg);
	}
}
