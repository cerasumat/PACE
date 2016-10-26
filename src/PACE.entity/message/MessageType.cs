using System;

namespace PACE.entity.message
{
	[Serializable]
	public enum MessageType
	{
		/// <summary>
		/// maps to one resource url
		/// </summary>
		Url,
		/// <summary>
		/// maps to one method of service call
		/// </summary>
		Service,
		/// <summary>
		/// maps to one method of search call
		/// </summary>
		Search,
		/// <summary>
		/// maps to internal method call
		/// </summary>
		Method,
		/// <summary>
		/// maps to one SQL statement
		/// </summary>
		Sql,
		/// <summary>
		/// maps to one cache access
		/// </summary>
		Cache,
		/// <summary>
		/// maps to Exception 
		/// </summary>
		Error
	}
}
