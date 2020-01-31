using System;

namespace PrankChat.Mobile.Core.Exceptions.Network
{
    public class AuthenticationProblemDetails : Exception
    {
        public int Status { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }
    }
}
