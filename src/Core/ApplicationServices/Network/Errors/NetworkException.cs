using System;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Errors
{
    public class NetworkException : ApplicationException
    {
        public NetworkException()
            : base() { }

        public NetworkException(string message)
            : base(message) { }

        public NetworkException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NetworkException(string message, Exception innerException)
            : base(message, innerException) { }

        public NetworkException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
