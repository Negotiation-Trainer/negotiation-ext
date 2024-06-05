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

    #region HttpRequest Methods
    /// <summary>
    /// Authenticated the player with a game password, and sets the session token for use when calling the back office.
    /// </summary>
    /// <param name="callback">The callback from the Http Call</param>
    /// <returns>A Post request that does a callback to the given method</returns>
    public IEnumerator Authenticate(Action<string> callback)
    {
        string path = "authenticate";
        return Post(path, _headers, new SessionPassword(gamePassword), callback);
    }
    
    /// <summary>
    /// Sets the Session Token return from the authentication call
    /// </summary>
    /// <param name="tokenResponse">The string that was returned from the Http Call</param>
    public void SetToken(string tokenResponse)
    {
        TokenResponse response = JsonUtility.FromJson<TokenResponse>(tokenResponse);
        _headers.Add("Authorization", response.token);
    }
    
    /// <summary>
    /// Debug method to get the current auth token
    /// </summary>
    /// <returns>Current Authorization Header</returns>
    public string Debug_GetAuth()
    {
        return _headers["Authorization"];
    }

    public IEnumerator GetGameConfig(Action<string> callback)
    {
        string path = "game-config";

        return Post(path, _headers, new SessionPassword(gamePassword), callback);
    }
    
    /// <summary>
    /// Sends a POST request to the back office to convert the user input to a trade deal.
    /// </summary>
    /// <param name="userTextInput">The user input</param>
    /// <param name="callback">The call back method</param>
    /// <returns>A Post request that does a callback to the given method</returns>
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
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param> //TODO: Change this to reasonToAccept
    /// <param name="callback">The callback func after completing</param>
    /// <returns>A string response from the POST request.</returns>
    public IEnumerator Accept(string speakerStyle, Trade tradeData, string reasonToAccept, Action<string> callback)
    {
        string path = $"chat/accept-deal?speakerStyle={speakerStyle}&reason={reasonToAccept}";
        
        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }
    
    /// <summary>
    /// Sends a POST request to reject a trade deal.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline a trade deal</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator Reject(string speakerStyle, Trade tradeData, string reasonToDecline, Action<string> callback)
    {
        string path = $"chat/reject-deal?speakerStyle={speakerStyle}&reason={reasonToDecline}";
        
        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }

    public IEnumerator CounterOffer(string speakerStyle, Trade tradeData, string reasonToDecline, Action<string> callback)
    {
        string path = $"chat/counter-offer?speakerStyle={speakerStyle}&reason={reasonToDecline}";
        
        // Send the POST request and return the response
        return Post(path, _headers, tradeData, callback);
    }
    #endregion

    #region Call Back Methods
    
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
    
    public GameConfig ConfigToString(string response)
    {
        // Convert the JSON response to a Trade object
        GameConfig? config = JsonConvert.DeserializeObject<GameConfig>(response);
        // If the conversion failed, throw an exception
        if (config == null)
        {
            throw new UserInputException("Could not convert the data to config");
        }
    
        return config;
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
    
    /// <summary>
    /// Converts the JSON string to a chat message object, when the user accepts a trade deal.
    /// </summary>
    /// <param name="response">JSON String from the Http Request</param>
    /// <returns>The Chat Message in a ChatMessage object</returns>
    /// <exception cref="UserInputException">Thrown when the message could not be converted</exception>
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
    
    /// <summary>
    /// Converts the JSON string to a chat message object, when the user rejects a trade deal.
    /// </summary>
    /// <param name="response">JSON String from the http Request</param>
    /// <returns>The Chat Message in a ChatMessage object</returns>
    /// <exception cref="UserInputException">Thrown when the message could not be converted</exception>
    public ChatMessage RejectDeal(string response)
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

    public ChatMessage CounterOffer(string response)
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

    //TODO: Proposed method to replace the above chat conversion methods
    public ChatMessage ConvertResponseToChatMessage(string response)
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
    
    #endregion
}