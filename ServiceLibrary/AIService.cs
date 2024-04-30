using System;
using System.Collections;
using System.Collections.Generic;
using ModelLibrary;
using ModelLibrary.Exceptions;
using Newtonsoft.Json;
using UnityHttpClients;

namespace ServiceLibrary;

/// <summary>
/// Service class for AI operations.
/// This class is responsible for handling AI related operations such as accepting and rejecting trade deals.
/// </summary>
public class AIService
{
    // public Trade ConvertToTrade(string response)
    // {
    //     // Convert the JSON response to a Trade object
    //     Trade? trade = JsonConvert.DeserializeObject<Trade>(response);
    //
    //     // If the conversion failed, throw an exception
    //     if (trade == null)
    //     {
    //         throw new UserInputException("Could not convert the user input to a trade deal.");
    //     }
    //
    //     return trade;
    // }
    //
    // public DealReturnMessage AcceptDeal(string response)
    // {
    //     // // Convert the JSON response to a DealReturnMessage object
    //     DealReturnMessage? message = JsonConvert.DeserializeObject<DealReturnMessage>(response);
    //     //
    //     // // If the conversion failed, throw an exception
    //     if (message == null)
    //     {
    //         throw new UserInputException("Could not convert the user input to a trade deal.");
    //     }
    //     
    //     // return dealReturnMessage;
    //     return message;
    // }
    //
    // public DealReturnMessage RejectDeal(string response)
    // {
    //
    //     // // Convert the JSON response to a DealReturnMessage object
    //     DealReturnMessage? message = JsonConvert.DeserializeObject<DealReturnMessage>(response);
    //     //
    //     // // If the conversion failed, throw an exception
    //     // if (message == null)
    //     // {
    //     //     throw new UserInputException("Could not convert the user input to a trade deal.");
    //     // }
    //     
    //     // return dealReturnMessage;
    //     return message;
    // }
}