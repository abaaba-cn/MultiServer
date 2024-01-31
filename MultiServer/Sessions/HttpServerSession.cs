using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using TcpServer.Controllers;

namespace TcpServer.Sessions;

class CommonCache
{
    public static CommonCache GetInstance()
    {
        if (_instance == null)
            _instance = new CommonCache();
        return _instance;
    }

    public string GetAllCache()
    {
        var result = new StringBuilder();
        result.Append("[\n");
        foreach (var item in _cache)
        {
            result.Append("  {\n");
            result.AppendFormat($"    \"key\": \"{item.Key}\",\n");
            result.AppendFormat($"    \"value\": \"{item.Value}\",\n");
            result.Append("  },\n");
        }

        result.Append("]\n");
        return result.ToString();
    }

    public bool GetCacheValue(string key, out string value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void PutCacheValue(string key, string value)
    {
        _cache[key] = value;
    }

    public bool DeleteCacheValue(string key, out string value)
    {
        return _cache.TryRemove(key, out value);
    }

    private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();
    private static CommonCache _instance;
}

public class HttpServerSession : HttpSession
{
    public HttpServerSession(HttpServer server) : base(server)
    {
    }

    protected override void OnReceivedRequest(HttpRequest request)
    {
        HttpController.HandleRequest(request, Response,this);
    }

    protected override void OnReceivedRequestError(HttpRequest request, string error)
    {
        Console.WriteLine($"Request error: {error}");
    }

    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"HTTP session caught an error: {error}");
    }
}