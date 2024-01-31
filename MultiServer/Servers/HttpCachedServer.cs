using TcpServer.Sessions;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace TcpServer.Servers
{
    internal class HttpCacheServer : HttpServer
    {
        // HTTP server port
        private int _port = 8080;
        // HTTP server content path
        private string _www = "/";

        public HttpCacheServer(IPAddress address, int port) : base(address, port)
        {
            this._port = port;
        }

        protected override TcpSession CreateSession() { return new HttpServerSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"HTTP session caught an error: {error}");
        }

        public void Run()
        {
            Console.WriteLine($"HTTP server port: {_port}");
            Console.WriteLine($"HTTP server static content path: {_www}");
            Console.WriteLine($"HTTP server website: http://localhost:{_port}/api/index.html");
            Console.WriteLine();

            this.AddStaticContent(_www, "/api");
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
                }
            }
            // Stop the server
            Console.Write("Server stopping...");
            this.Stop();
            Console.WriteLine("Done!");
        }
    }
}