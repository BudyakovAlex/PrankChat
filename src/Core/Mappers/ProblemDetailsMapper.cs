using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Data.Dtos;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class ProblemDetailsMapper
    {
        public static ProblemDetailsException Map(this ProblemDetailsDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new ProblemDetailsException(
                dto.CodeError,
                dto.Title,
                dto.Message,
                dto.InvalidParams,
                dto.StatusCode,
                dto.Type);
        }
    }
}