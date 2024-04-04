using System.Collections.Generic;
using System.Linq;

namespace UnityHttpClients;

public class BackOfficeHttpClient: AbstractHttpClient
{
    private Dictionary<string, string> _headers = new();
    
    public BackOfficeHttpClient(string baseUrl, string sessionToken) : base(baseUrl)
    {
        _headers.Add("token", sessionToken);
    }

    public string Chat(string prompt, Dictionary<string, string> extraHeaders)
    {
        
       // Merge the two header dictonaries
       var mergedHeaders = _headers.Concat(extraHeaders).ToDictionary(x => x.Key, x => x.Value);
       
       string path = $"chat?prompt={prompt}";
       
       return Get(path, mergedHeaders);
    }
}