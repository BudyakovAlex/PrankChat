using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Exceptions.Network
{
    public class InternalServerProblemDetails : Exception
    {
        public InternalServerProblemDetails(string message) : base(message)
        { }
    }
}
