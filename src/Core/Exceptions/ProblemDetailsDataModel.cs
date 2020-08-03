using PrankChat.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Exceptions
{
    public class ProblemDetailsDataModel : ApplicationException
    {
        public ProblemDetailsDataModel() : base(string.Empty)
        {
        }

        public ProblemDetailsDataModel(string message) : base(message)
        {
        }

        public string CodeError { get; set; }

        public string Title { get; set; }

        public string MessageServerError { get; set; }

        public List<ItemInvalidParameter> InvalidParams { get; set; }

        public int? StatusCode { get; set; }

        public string Type { get; set; }

        public override string Message => InvalidParams?.FirstOrDefault()?.Reason ?? MessageServerError;
    }
}
