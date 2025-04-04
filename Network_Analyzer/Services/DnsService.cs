using System.Net;

namespace Network_Analyzer.Services;

public class DnsService
{
    public string GetDnsInfo(string host)
    {
        try
        {
            var entry = Dns.GetHostEntry(host);
            string addresses = string.Join(", ", entry.AddressList.Select(x => x.ToString()));
            // Логирование успешного запроса
            Console.WriteLine($"DNS lookup successful for {host}: {addresses}");
            return addresses;
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine($"DNS lookup failed for {host}: {ex.Message}");
            return "DNS lookup failed";
        }
    }
}