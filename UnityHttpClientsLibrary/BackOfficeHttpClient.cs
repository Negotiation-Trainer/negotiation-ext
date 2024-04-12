using System.Collections.Generic;
using System.Linq;
using ModelLibrary;

namespace UnityHttpClients;

/// <summary>
/// HTTP client for the back office.
/// </summary>
public class BackOfficeHttpClient: AbstractHttpClient
{
    private Dictionary<string, string> _headers = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BackOfficeHttpClient"/> class.
    /// </summary>
    /// <param name="baseUrl">The base URL of the API. Do not include a trailing slash.</param>
    /// <param name="sessionToken">The session token for authentication.</param>
    public BackOfficeHttpClient(string baseUrl, string sessionToken) : base(baseUrl)
    {
        _headers.Add("token", sessionToken);
    }

    /// <summary>
    /// Sends a AI prompt to the server.
    /// </summary>
    /// <param name="prompt">The chat message to send.</param>
    /// <param name="extraHeaders">Additional headers for the request.</param>
    /// <returns>A string response from the server.</returns>
    public string Chat(string prompt, Dictionary<string, string> extraHeaders)
    {
        
       // Merge the two header dictionaries
       var mergedHeaders = _headers.Concat(extraHeaders).ToDictionary(x => x.Key, x => x.Value);
       
       string path = "chat";
       
       return Post(path, mergedHeaders, new InputPromptBody(prompt));
    }
}