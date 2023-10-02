using Newtonsoft.Json;

namespace TransactionManager;

public class WeatherForecast
{
    public const string clientID = "0c6a4987ca4f59d7626d";
    public const string redirect_uri = "http://transaction-manager.tailf6119.ts.net:4200/callback";
    public string ClientId
    {
        get { return clientID; }
    }
    public string RedirectURI
    {
        get { return redirect_uri; }
    }
}

