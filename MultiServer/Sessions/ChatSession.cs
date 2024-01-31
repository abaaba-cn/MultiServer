using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using TcpServer.Domain;

namespace TcpServer.Sessions;

class ChatSession : TcpSession
{
    public ChatSession(NetCoreServer.TcpServer server) : base(server) {}

    protected override void OnConnected()
    {
        Console.WriteLine($"Chat TCP session with Id {Id} connected!");
        var address = IPEndPoint.Parse(this.Socket.RemoteEndPoint.ToString());
        Program.Clients.Add(new HostModel()
        {
            IPEndPoint = address,
            LastActiveTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        // Send invite message
        string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
        SendAsync(message);
    }

    protected override void OnDisconnected()
    {
        Program.Clients.Remove(Program.Clients.FirstOrDefault(x => x.IPEndPoint.ToString() == this.Socket.RemoteEndPoint.ToString()));
        Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        Console.WriteLine("Incoming: " + message);

        // Multicast message to all connected sessions
        Server.Multicast(message);

        // If the buffer starts with '!' the disconnect the current session
        if (message == "!")
            Disconnect();
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat TCP session caught an error with code {error}");
    }
}