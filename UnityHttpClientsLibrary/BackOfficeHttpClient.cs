using System.Collections.Generic;
using ModelLibrary;

namespace UnityHttpClients;

/// <summary>
/// HTTP client for the back office.
/// This class is responsible for making HTTP requests to the back office API.
/// </summary>
public class BackOfficeHttpClient: AbstractHttpClient
{
    // Headers to be sent with each request
    private Dictionary<string, string> _headers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="BackOfficeHttpClient"/> class.
    /// </summary>
    /// <param name="baseUrl">The base URL of the API. Do not include a trailing slash.</param>
    /// <param name="sessionPwd">The session token for authentication.</param>
    public BackOfficeHttpClient(string baseUrl, string sessionPwd) : base(baseUrl)
    {
        // Add the session token to the headers
        _headers.Add("Authorization", $"{sessionPwd})");
    }
    
    public string Authenticate(string sessionPwd)
    {
        string path = "authenticate";
        return Post(path, _headers, new SessionPassword(sessionPwd));
    }

    /// <summary>
    /// Sends a POST request to accept a trade deal.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <returns>A string response from the POST request.</returns>
    public string Accept(string speakerStyle, Trade tradeData, string reasonToDecline)
    {
        string path = $"chat/accept-deal?speakerStyle={speakerStyle}&reasonToDecline={reasonToDecline}";

        // Send the POST request and return the response
        return Post(path, _headers, tradeData);
    }

    /// <summary>
    /// Sends a POST request to decline a trade deal.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <returns>A string response from the POST request.</returns>
    public string Decline(string speakerStyle, Trade tradeData, string reasonToDecline)
    {
        string path = $"chat/reject-deal?speakerStyle={speakerStyle}&reasonToDecline={reasonToDecline}";

        // Send the POST request and return the response
        return Post(path, _headers, tradeData);
    }

    /// <summary>
    /// Converts a user's text input to a Trade object.
    /// </summary>
    /// <param name="userTextInput">The user's text input.</param>
    /// <returns>A Trade object that represents the user's text input.</returns>
    public string ConvertToTrade(string userTextInput)
    {
        string path = "chat/convert-to-trade";

        // Send the POST request and get the response
        return Post(path, _headers, new InputPromptBody(userTextInput));
    }
}