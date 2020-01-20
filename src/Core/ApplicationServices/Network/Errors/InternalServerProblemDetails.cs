using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Errors
{
    public class InternalServerProblemDetails : Exception
    {
        public InternalServerProblemDetails(string message) : base(message)
        { }
    }
}
