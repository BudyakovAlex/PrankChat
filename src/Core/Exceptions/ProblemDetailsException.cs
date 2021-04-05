using PrankChat.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrankChat.Mobile.Core.Exceptions
{
    public class ProblemDetailsException : ApplicationException
    {
        public ProblemDetailsException() : base(string.Empty)
        {
        }

        public ProblemDetailsException(string message) : base(message)
        {
        }

        public ProblemDetailsException(string codeError,
                                       string title,
                                       string messageServerError,
                                       List<ItemInvalidParameter> invalidParams,
                                       int? statusCode,
                                       string type)
        {
            CodeError = codeError;
            Title = title;
            MessageServerError = messageServerError;
            InvalidParams = invalidParams;
            StatusCode = statusCode;
            Type = type;
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
