using System;
using System.Collections.Generic;
using System.Linq;
using PrankChat.Mobile.Core.Models;

namespace PrankChat.Mobile.Core.Exceptions
{
    public class ProblemDetailsDataModel : ApplicationException
    {
        public string CodeError { get; set; }

        public string Title { get; set; }

        public string MessageServerError { get; set; }

        public List<ItemInvalidParameter> InvalidParams { get; set; }

        public int? StatusCode { get; set; }

        public string Type { get; set; }

        public ProblemDetailsDataModel() : base(string.Empty) { }

        public ProblemDetailsDataModel(string message) : base(message)
        {
        }

        public override string Message
        {
            get
            {
                var text = new List<string> { MessageServerError };
                if (InvalidParams != null && InvalidParams.Count > 0)
                {
                    text = text.Concat(InvalidParams.Select(itemInvalidParameter => itemInvalidParameter.ToString())).ToList();
                }

                return string.Join(Environment.NewLine, text);
            }
        }
    }
}
