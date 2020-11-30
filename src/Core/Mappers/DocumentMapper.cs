using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class DocumentMapper
    {
        public static DocumentDataModel Map(this DocumentApiModel documentApiModel)
        {
            return new DocumentDataModel(documentApiModel.Id,
                                         documentApiModel.Path);
        }

        public static DocumentDataModel Map(this DataApiModel<DocumentApiModel> documentApiModel)
        {
            return new DocumentDataModel(documentApiModel.Data.Id,
                                         documentApiModel.Data.Path);
        }
    }
}