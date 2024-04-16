using ModelLibrary;
using ModelLibrary.Exceptions;
using Models;
using Newtonsoft.Json;
using UnityHttpClients;

namespace ServiceLibrary;

/// <summary>
/// Service class for AI operations.
/// This class is responsible for handling AI related operations such as accepting and rejecting trade deals.
/// </summary>
public class AIService
{
    // HTTP client for the back office
    private readonly BackOfficeHttpClient _backOfficeHttpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AIService"/> class.
    /// </summary>
    /// <param name="baseUrl">The base URL of the API. Do not include a trailing slash.</param>
    /// <param name="sessionToken">The session token for authentication.</param>
    public AIService(string baseUrl, string sessionToken)
    {
        _backOfficeHttpClient = new BackOfficeHttpClient(baseUrl, sessionToken);
    }

    /// <summary>
    /// Accepts a proposed trade deal and generates a response message.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToAccept">The reason to accept the trade deal.</param>
    /// <returns>A DealReturnMessage object containing the response message.</returns>
    public DealReturnMessage AcceptDeal(string speakerStyle, Trade tradeData, string reasonToAccept)
    {
        // Send the accept request and get the response
        var response = _backOfficeHttpClient.Accept(speakerStyle, tradeData, reasonToAccept);

        // Convert the JSON response to a DealReturnMessage object
        var dealReturnMessage = JsonConvert.DeserializeObject<DealReturnMessage>(response);

        // If the conversion failed, throw an exception
        if (dealReturnMessage == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }

        return dealReturnMessage;
    }

    /// <summary>
    /// Rejects a proposed trade deal and generates a response message.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <returns>A DealReturnMessage object containing the response message.</returns>
    public DealReturnMessage RejectDeal(string speakerStyle, Trade tradeData, string reasonToDecline)
    {
        // Send the reject request and get the response
        var response = _backOfficeHttpClient.Decline(speakerStyle, tradeData, reasonToDecline);

        // Convert the JSON response to a DealReturnMessage object
        var dealReturnMessage = JsonConvert.DeserializeObject<DealReturnMessage>(response);

        // If the conversion failed, throw an exception
        if (dealReturnMessage == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }

        return dealReturnMessage;
    }

    /// <summary>
    /// Converts a user's text input to a Trade object.
    /// </summary>
    /// <param name="userTextInput">The user's text input.</param>
    /// <returns>A Trade object that represents the user's text input.</returns>
    public Trade ConvertToTrade(string userTextInput)
    {
        // Send the convert request and get the response
        var response = _backOfficeHttpClient.ConvertToTrade(userTextInput);

        // Convert the JSON response to a Trade object
        Trade? trade = JsonConvert.DeserializeObject<Trade>(response);

        // If the conversion failed, throw an exception
        if (trade == null)
        {
            throw new UserInputException("Could not convert the user input to a trade deal.");
        }

        return trade;
    }
}