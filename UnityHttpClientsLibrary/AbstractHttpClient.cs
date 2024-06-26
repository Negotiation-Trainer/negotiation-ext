using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UnityHttpClients
{
    /// <summary>
    /// Abstract base class for HTTP clients.
    /// </summary>
    public abstract class AbstractHttpClient
    {
        private readonly string _baseUrl;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractHttpClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the API. Do not include a trailing slash.</param>
        protected AbstractHttpClient(string baseUrl)
        {
            // remove trailing slash
            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Tests the GET method.
        /// </summary>
        /// <returns>A string response from the GET request.</returns>
        public string TestGetMethod()
        {
            return Get("https://catfact.ninja/fact", new Dictionary<string, string>());
        }
        
        /// <summary>
        /// Executes a GET request to an API endpoint.
        /// </summary>
        /// <param name="pathUrl">The path to the API endpoint, e.g., /chat.</param>
        /// <param name="headers">The headers for the request, like auth headers, etc.</param>
        /// <returns>A string response from the GET request.</returns>
        protected string Get(string pathUrl, Dictionary<string, string> headers)
        {
            UnityWebRequest request = UnityWebRequest.Get($"{_baseUrl}/{pathUrl}");
            
            foreach (KeyValuePair<string, string> header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
            
            var operation = request.SendWebRequest();

            // Wait for the request to complete
            while (!operation.isDone) { }

            string resultText;
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    resultText = request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    resultText = $"Connection Error: {request.error}";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    resultText = $"Protocol Error: {request.error}";
                    break;
                default:
                    resultText = "Unknown error";
                    break;
            }

            return resultText;
        }

        /// <summary>
        /// Executes a POST request to an API endpoint.
        /// </summary>
        /// <typeparam name="T">The type of the body object.</typeparam>
        /// <param name="pathUrl">The path to the API endpoint, e.g., /chat.</param>
        /// <param name="headers">The headers for the request, like auth headers, etc.</param>
        /// <param name="body">The body of the POST request.</param>
        /// <param name="callback">The callback to invoke</param>
        /// <returns>A string response from the POST request.</returns>
        
        protected IEnumerator Post<T>(string pathUrl, Dictionary<string, string> headers, T body, Action<string> callback)
        {
            string jsonBody = JsonConvert.SerializeObject(body, new StringEnumConverter());
            
            Debug.Log("Json body: " + jsonBody);
        
            using (UnityWebRequest wr = UnityWebRequest.Post($"{_baseUrl}/{pathUrl}", jsonBody, "application/json"))
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    wr.SetRequestHeader(header.Key, header.Value);
                }
            
                yield return wr.SendWebRequest();
            
                if (wr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(wr.error);
                    Debug.Log(wr.downloadHandler.text);
                }
                else
                {
                    Debug.Log(wr.downloadHandler.text);
                }
            
                callback.Invoke(wr.downloadHandler.text);
            }
        }
    }
}