using System.Net;

namespace Network_Analyzer.Services;

public class AddresService
{
    public string GetAddressType(string address)
    {
        if (IPAddress.TryParse(address, out var ipAddress))
        {
            if (ipAddress.Equals(IPAddress.Loopback))
            {
                return "Loopback";
            }
            return "Public";
        }
        return "Invalid IP Address";
    }
}