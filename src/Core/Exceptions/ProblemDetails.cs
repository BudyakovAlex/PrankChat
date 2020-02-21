using System;
using System.Collections.Generic;
using System.Linq;
using PrankChat.Mobile.Core.Models;

namespace PrankChat.Mobile.Core.Exceptions
{
    public class ProblemDetails : ApplicationException
    {
        public string CodeError { get; set; }

        public string Title { get; set; }

        public string MessageServerError { get; set; }

        public List<ItemInvalidParameter> InvalidParams { get; set; }

        public int? StatusCode { get; set; }

        public string Type { get; set; }

        public ProblemDetails() : base(string.Empty) { }

        public ProblemDetails(string message) : base(message)
        {
        }

        public override string Message
        {
            get
            {
                var arrayProblems = new List<string> { Title, MessageServerError, base.Message };
                if (InvalidParams != null && InvalidParams.Count > 0)
                {
                    arrayProblems = arrayProblems.Concat(InvalidParams.Select(x => x.ToString())).ToList();
                }

                return string.Join(Environment.NewLine, arrayProblems);
            }
        }
    }
}
