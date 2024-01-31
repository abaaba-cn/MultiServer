// See https://aka.ms/new-console-template for more information

using System.Collections.ObjectModel;
using System.Net;
using TcpServer.Domain;
using TcpServer.Servers;

namespace TcpServer;

internal static class Program
{
    public static ObservableCollection<HostModel> Clients { get;  set; } = new ObservableCollection<HostModel>();
    
    static void Main(string[] args)
    {
        CreateHttpServer();
        CreateChatServer();
        Console.ReadKey();
    }

    static void CreateChatServer()
    {
        Task.Run(() =>
        {
            var chatServer = new ChatServer(IPAddress.Any, 1111);
            chatServer.Run();
        });
    }

    static void CreateHttpServer()
    {
        Task.Run(() =>
        {
            var httpServer = new HttpCacheServer(IPAddress.Any, 8838);
            httpServer.Run();
        });
    }
}