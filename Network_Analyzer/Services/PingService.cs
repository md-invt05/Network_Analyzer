using System.Net.NetworkInformation;

namespace Network_Analyzer.Services;

public class PingService
{
    public async Task<bool> PingHostAsync(string host)
    {
        using var ping = new Ping();
        try
        {
            PingReply reply = await ping.SendPingAsync(host);
            return reply.Status == IPStatus.Success;
        }
        catch (Exception ex)
        {
            // Логируем/выводим ошибку
            Console.WriteLine($"Ping error: {ex.Message}");
            return false;
        }
    }
}