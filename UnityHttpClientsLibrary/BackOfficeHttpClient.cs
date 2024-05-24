using System;
using System.Collections;
using System.Collections.Generic;
using ModelLibrary;
using ModelLibrary.Exceptions;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityHttpClients;

/// <summary>
/// HTTP client for the back office.
/// This class is responsible for making HTTP requests to the back office API.
/// </summary>
public class BackOfficeHttpClient: AbstractHttpClient
{
    // Headers to be sent with each request
    private Dictionary<string, string> _headers = new();

    public string gamePassword { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackOfficeHttpClient"/> class.
    /// </summary>
    /// <param name="baseUrl">The base URL of the API. Do not include a trailing slash.</param>
    /// <param name="gamePassword">The session token for authentication.</param>
    public BackOfficeHttpClient(string baseUrl, string gamePassword) : base(baseUrl)
    {
        //Set up the headers
        _headers.Add("Content-Type", "application/json");
        _headers.Add("Accept", "application/json");
        
        this.gamePassword = gamePassword;
    }

    public IEnumerator Authenticate(Action<string> callback)
    {
        string path = "authenticate";
        return Post(path, _headers, new SessionPassword(gamePassword), callback);
    }
    
    public void SetToken(string tokenResponse)
    {
        TokenResponse response = JsonUtility.FromJson<TokenResponse>(tokenResponse);
        _headers.Add("Authorization", response.token);
    }
    
    public string Debug_GetAuth()
    {
        return _headers["Authorization"];
    }
    
    public IEnumerator ConvertToTrade(string userTextInput, Action<string> callback)
    {
        string path = "chat/convert-to-trade";
    
        // Return the POST request to complete
        return Post(path, _headers, new InputPromptBody(userTextInput), callback);
    }
    
    public IEnumerator ConvertToChat(string speakerStyle, Trade tradeData, Action<string> callback)
    {
        string path = $"chat/convert-to-chat?speakerStyle={speakerStyle}";

        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }
    
    /// <summary>
    /// Sends a POST request to accept a trade deal.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <param name="callback">The callback func after completing</param>
    /// <returns>A string response from the POST request.</returns>
    public IEnumerator Accept(string speakerStyle, Trade tradeData, string reasonToDecline, Action<string> callback)
    {
        string path = $"chat/accept-deal?speakerStyle={speakerStyle}&reason={reasonToDecline}";
        
        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }
    
    public IEnumerator Reject(string speakerStyle, Trade tradeData, string reasonToDecline, Action<string> callback)
    {
        string path = $"chat/reject-deal?speakerStyle={speakerStyle}&reason={reasonToDecline}";
        
        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }
    
    
    public Trade ConvertToTrade(string response)
    {
        // Convert the JSON response to a Trade object
        Trade? trade = JsonConvert.DeserializeObject<Trade>(response);
        // If the conversion failed, throw an exception
        if (trade == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }
    
        return trade;
    }
    
    public ChatMessage TradeToChat(string response)
    {
        // Convert the JSON response to a ChatMessage object
        ChatMessage? message = JsonUtility.FromJson<ChatMessage>(response);
        
        // If the conversion failed, throw an exception
        if (message == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }
        
        // return ChatMessage;
        return message;
    }
    
    
    public ChatMessage AcceptDeal(string response)
    {
        // Convert the JSON response to a ChatMessage object
        ChatMessage? message = JsonUtility.FromJson<ChatMessage>(response);
        
        // If the conversion failed, throw an exception
        if (message == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }
        
        // return ChatMessage;
        return message;
    }
    
    public ChatMessage RejectDeal(string response)
    {

        // // Convert the JSON response to a ChatMessage object
        ChatMessage? message = JsonUtility.FromJson<ChatMessage>(response);
        
        // If the conversion failed, throw an exception
        if (message == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }
        
        // return ChatMessage;
        return message;
    }
}