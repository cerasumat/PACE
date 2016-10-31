using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PACE.Utility
{
	public class NetworkInterfaceManager
	{
		public static string GetIp()
		{
			string hostName = Dns.GetHostName();
			var ip = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
			return null == ip ? null : ip.ToString();
		}
	}
}
