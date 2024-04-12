using System.Collections.Generic;
using Models;
using UnityHttpClients;

namespace ServiceLibrary;

public class AIService(string baseUrl, string sessionToken)
{
    private readonly BackOfficeHttpClient _backOfficeHttpClient = new(baseUrl, sessionToken);

    /// <summary>
    /// Accepts a proposed trade deal and generates a response message.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <returns>A string response message.</returns>
    public string AcceptDeal(string speakerStyle, Trade tradeData, string reasonToDecline)
    {
        var prompt = $"accept this trade and speak {speakerStyle} to this deal: {tradeData.RequestedAmount} {tradeData.RequestedItem} for {tradeData.OfferedAmount} {tradeData.OfferedItem}. in max 3 sentences. It’s good because {reasonToDecline}";
        return _backOfficeHttpClient.Chat(prompt, new Dictionary<string, string>());
    }
    
    /// <summary>
    /// Rejects a proposed trade deal and generates a response message.
    /// </summary>
    /// <param name="speakerStyle">The style in which the speaker should respond.</param>
    /// <param name="tradeData">The data of the trade deal.</param>
    /// <param name="reasonToDecline">The reason to decline the trade deal.</param>
    /// <returns>A string response message.</returns>
    public string RejectDeal(string speakerStyle, Trade tradeData, string reasonToDecline)
    {
        
        var prompt = $"reject this trade and speak {speakerStyle} to this deal: {tradeData.RequestedAmount} {tradeData.RequestedItem} for {tradeData.OfferedAmount} {tradeData.OfferedItem}. in max 3 sentences. It’s bad because {reasonToDecline}";
        return _backOfficeHttpClient.Chat(prompt, new Dictionary<string, string>());
    }
}