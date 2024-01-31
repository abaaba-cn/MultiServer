using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using TcpServer.Sessions;

namespace TcpServer.Servers;

internal class ChatServer : NetCoreServer.TcpServer
{
    private int _port = 1111;
    public ChatServer(IPAddress address, int port) : base(address, port) {}

    protected override TcpSession CreateSession() { return new ChatSession(this); }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Chat TCP server caught an error with code {error}");
    }

    public void Run()
    {
        Console.WriteLine($"TCP server port: {_port}");
        Console.WriteLine();
        // Start the server
        Console.Write("Server starting...");
        this.Start();
        Console.WriteLine("Done!");
        Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

        // Perform text input
        for (;;)
        {
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;

            // Restart the server
            if (line == "!")
            {
                Console.Write("Server restarting...");
                this.Restart();
                Console.WriteLine("Done!");
                continue;
            }

            // Multicast admin message to all sessions
            line = "(admin) " + line;
            this.Multicast(line);
        }

        // Stop the server
        Console.Write("Server stopping...");
        this.Stop();
        Console.WriteLine("Done!");
    }
}