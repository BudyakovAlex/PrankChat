using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.BusinessServices.Logger
{
    public interface ILogger
    {
        Task WriteRequestInfoAsync(DateTime dateTime, string tag, string message, List<Parameter> parameters = null);

        Task WriteResponseInfoAsync(DateTime dateTime, HttpStatusCode statusCode, string tag, string message, string content, List<Parameter> headers = null);

        Task LogEventAsync(DateTime dateTime, string tag, string extraParameters = null);

        Task ClearLogAsync();

        string LogFilePath { get; }
    }
}
