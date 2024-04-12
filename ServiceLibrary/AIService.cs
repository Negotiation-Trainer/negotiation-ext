using UnityHttpClients;

namespace ServiceLibrary;

public class AIService(string baseUrl, string sessionToken)
{
    private BackOfficeHttpClient _backOfficeHttpClient = new(baseUrl, sessionToken);
    
    
}