using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CardMapper
    {
        public static Card Map(this CardDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Card(
                dto.Id,
                dto.Number,
                dto.CardUserName);
        }
    }
}