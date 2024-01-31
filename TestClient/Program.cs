// See https://aka.ms/new-console-template for more information

using TestClient.Clients;

namespace TestClient
{
    internal static class Program
    {
        static void Main()
        {
            var client = new ChatClient("127.0.0.1", 1111);
            client.Run();
            Console.ReadKey();
        }
    }
}