using System;
using System.Collections.Generic;
using System.Linq;
using PrankChat.Mobile.Core.Models;

namespace PrankChat.Mobile.Core.Exceptions.Network
{
    public class InternalServerProblemDetails : Exception
    {
        public string CodeError { get; set; }

        public string Title { get; set; }

        public string MessageServerError { get; set; }

        public List<ItemInvalidParameter> InvalidParams { get; set; }

        public int? StatusCode { get; set; }

        public string Type { get; set; }

        public InternalServerProblemDetails() : base(string.Empty) { }

        public InternalServerProblemDetails(string message) : base(message)
        {
        }

        public override string Message
        {
            get
            {
                var arrayProblems = new[] { Title, MessageServerError, base.Message }.Concat(InvalidParams?.Select(x => x.ToString()));
                return string.Join(Environment.NewLine, arrayProblems);
            }
        }
    }
}
