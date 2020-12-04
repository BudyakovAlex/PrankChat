using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Models.Api;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class ProblemDetailsMapper
    {
        public static ProblemDetailsDataModel Map(this ProblemDetailsApiModel problemDetailsApiModel)
        {
            if (problemDetailsApiModel is null)
            {
                return null;
            }

            return new ProblemDetailsDataModel(problemDetailsApiModel.CodeError,
                                               problemDetailsApiModel.Title,
                                               problemDetailsApiModel.Message,
                                               problemDetailsApiModel.InvalidParams,
                                               problemDetailsApiModel.StatusCode,
                                               problemDetailsApiModel.Type);
        }
    }
}