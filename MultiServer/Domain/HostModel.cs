using System.Net;

namespace TcpServer.Domain;

public class HostModel
{
    public int MemberId { get; set; }
    public IPEndPoint IPEndPoint { get; set; }
    public DateTime LastActiveTime { get; set; }
    public DateTime CreateTime { get; set; }
    public string Encoding { get; set; }
    public string Client { get; set; }
    public bool Expire { get; set; }

    public bool Equals(IPEndPoint obj)
    {
        return obj.Address.ToString() == IPEndPoint.Address.ToString() && obj.Port == IPEndPoint.Port;
    }
}