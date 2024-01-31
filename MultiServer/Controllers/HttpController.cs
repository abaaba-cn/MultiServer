using NetCoreServer;
using TcpServer.Sessions;

namespace TcpServer.Controllers;

public class HttpController
{
    public static void HandleRequest(HttpRequest request, HttpResponse response, HttpServerSession session)
    {
        if (request.Method == "HEAD")
            session.SendResponseAsync(response.MakeHeadResponse());
        
        if (request.Method == "GET")
            HandleGetRequest(request, response, session);

        if ((request.Method == "POST"))
            HandlePostRequest(request, response, session);

        if (request.Method == "DELETE")
            session.SendResponseAsync(response.MakeOkResponse());

        if (request.Method == "OPTIONS")
            session.SendResponseAsync(response.MakeOptionsResponse());
        
        if (request.Method == "TRACE")
            session.SendResponseAsync(response.MakeOkResponse());
    }

    public static void HandleGetRequest(HttpRequest request, HttpResponse response, HttpServerSession session)
    {
        string key = request.Url;
        key = Uri.UnescapeDataString(key);
        Console.WriteLine($"get : {key}");
        key = key.Replace("/api/cache", "", StringComparison.InvariantCultureIgnoreCase);
        key = key.Replace("/?key=", "", StringComparison.InvariantCultureIgnoreCase);

        if (request.Url.Contains("?key="))
        {
            session.SendResponseAsync(response.MakeGetResponse(key));
        }

        if (request.Url.Contains("/clients/list"))
        {
            session.SendResponseAsync(response.MakeGetResponse(Program.Clients.Count.ToString()));
        }

        session.SendResponseAsync(response.MakeGetResponse(key));

        if (string.IsNullOrEmpty(key))
        {
            session.SendResponseAsync(response.MakeGetResponse(CommonCache.GetInstance().GetAllCache(),"application/json; charset=UTF-8"));
        }
        else if (CommonCache.GetInstance().GetCacheValue(key, out var value))
        {
            session.SendResponseAsync(response.MakeGetResponse(value));
        }
        else
        {
            session.SendResponseAsync(response.MakeErrorResponse(404, "Required cache value was not found for the key: " + key));
        }
    }

    public static void HandlePostRequest(HttpRequest request, HttpResponse response, HttpServerSession session)
    {
        string key = request.Url;
        string value = request.Body;

        // Decode the key value
        key = Uri.UnescapeDataString(key);
        Console.WriteLine($"post : {key}");
        key = key.Replace("/api/cache/add", "", StringComparison.InvariantCultureIgnoreCase);
        key = key.Replace("?key=", "", StringComparison.InvariantCultureIgnoreCase);

        // Put the cache value
        CommonCache.GetInstance().PutCacheValue(key, value);
        // Response with the cache value
        session.SendResponseAsync(response.MakeOkResponse());
    }
}