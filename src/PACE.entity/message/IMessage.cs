using System;
using System.Collections.Generic;

namespace PACE.entity.message
{
	/// <summary>
	/// Runtime collected data interfaces
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// Initialized to be 'TRUE'.
		/// </summary>
		bool IsSuccess { get; }

		/// <summary>
		/// Return whether the Complete() method was called, false means not. 
		/// </summary>
		bool IsComplete { get; }

		/// <summary>
		/// Get the key-value pairs data
		/// </summary>
		IDictionary<string, string> Data { get; }

		/// <summary>
		/// Get the	message name.
		/// </summary>
		string Name { get;}

		/// <summary>
		/// Get the message status, "0" means success, otherwise error code.
		/// </summary>
		string Status { get; }

		/// <summary>
		/// Get the message type.
		/// </summary>
		MessageType Type { get; }

		/// <summary>
		/// Get the time stamp message was created.
		/// </summary>
		DateTime Timestamp { get; }

		/// <summary>
		/// add one or multiple key-value pairs to the message.
		/// </summary>
		/// ReSharper disable CSharpWarnings::CS1570
		/// <param name="keyValuePairs">key-value pairs like 'a=1&b=2&c=3...'</param>
		/// ReSharper restore CSharpWarnings::CS1570
		void AddData(string keyValuePairs);

		/// <summary>
		/// add one key-value pair to the message.
		/// </summary>
		/// <param name="key">key-value pair key</param>
		/// <param name="value">key-value pair value</param>
		void AddData(string key, string value);

		/// <summary>
		/// Set the message status.
		/// </summary>
		/// <param name="status">"0" means success, otherwise error code</param>
		void SetStatus(string status);

		/// <summary>
		/// Set the message status.
		/// </summary>
		/// <param name="e">exception and Derived exceptions</param>
		void SetStatus(Exception e);

		/// <summary>
		/// Set the complete status
		/// </summary>
		/// <param name="isComplete">true means completed.</param>
		void SetComplete(bool isComplete);

		/// <summary>
		/// Complete the message construction.
		/// </summary>
		void Complete();
	}
}
