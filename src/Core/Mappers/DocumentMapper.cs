using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class DocumentMapper
    {
        public static Document Map(this DocumentDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Document(dto.Id, dto.Path);
        }

        public static Document Map(this ResponseDto<DocumentDto> dto)
        {
            if (dto.Data is null)
            {
                return null;
            }

            return new Document(dto.Data.Id, dto.Data.Path);
        }
    }
}