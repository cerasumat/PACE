using System;
using System.Linq;
using System.Net;

namespace PACE.Utility
{
	public static class ExtensionMethods
	{
		public static Uri AppendPath(this Uri uri, string path)
		{
			var newPath = uri.AbsolutePath.TrimEnd(new[] { '/', ' ' }) + "/" + path;
			return new UriBuilder(uri.Scheme, uri.Host, uri.Port, newPath, uri.Query).Uri;
		}

		public static int ToInt32(this string str)
		{
			int result;
			if (int.TryParse(str, out result))
				return result;
			throw new InvalidCastException(str + " Can not parse to Int32.");
		}

		public static bool IsFailed(this HttpStatusCode code)
		{
			var _code = (int)code;
			return _code >= 400 || _code == 0;
		}

		public static bool IsSucceed(this HttpStatusCode code)
		{
			var _code = (int)code;
			return _code >= 200 && _code <= 203;	 // 2XX that greater than 203 means SUCCESS BUT NO CONTENT or PARTITIAL CONTENT
		}

		public static DateTime ToDateTime(this long milliseconds)
		{
			return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(milliseconds);
		}

		public static long ToMilliseconds(this DateTime time)
		{
			return (long)(time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalMilliseconds;
		}

		public static double ToKb(this long bytes)
		{
			return bytes / 1024d;
		}

		public static double ToMb(this long bytes)
		{
			return bytes / 1024d / 1024d;
		}

		public static double ToGb(this long bytes)
		{
			return bytes / 1024d / 1024d / 1024d;
		}

		public static bool IsNumeric(this Type t)
		{
			return !t.IsClass && !t.IsInterface && t.GetInterfaces().Any(i => i == typeof(IFormattable));
		}

		/// <summary>
		/// Milliseconds Convert to TimeSpan
		/// </summary>
		/// <param name="milliseconds">milliseconds</param>
		/// <returns>TimeSpan</returns>
		public static TimeSpan ToTimeSpan(this long milliseconds)
		{
			long temp = milliseconds / 1000;
			int second = (int)temp % 60;
			temp = temp / 60;
			int minite = (int)temp % 60;
			temp = temp / 60;
			int hour = (int)temp % 24;
			temp = temp / 24;
			return new TimeSpan((int)temp, hour, minite, second);
			// OR use the ticks, 1ms = 10000 ticks
			// return new TimeSpan(maxRunTime*10000);
		}
	}
}
