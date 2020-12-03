using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class VideoMapper
    {
        public static VideoDataModel Map(this VideoApiModel videoApiModel)
        {
            if (videoApiModel.User.Data is null ||
                videoApiModel.Customer.Data is null)
            {
                return null;
            }

            return new VideoDataModel(videoApiModel.Id,
                                      videoApiModel.Title,
                                      videoApiModel.Description,
                                      videoApiModel.Poster,
                                      videoApiModel.Status,
                                      videoApiModel.ViewsCount ?? 0,
                                      videoApiModel.RepostsCount ?? 0,
                                      videoApiModel.LikesCount ?? 0,
                                      videoApiModel.DislikesCount ?? 0,
                                      videoApiModel.CommentsCount ?? 0,
                                      videoApiModel.StreamUri,
                                      videoApiModel.PreviewUri,
                                      videoApiModel.MarkedStreamUri,
                                      videoApiModel.ShareUri,
                                      videoApiModel.IsLiked,
                                      videoApiModel.IsDisliked,
                                      videoApiModel.OrderCategory,
                                      videoApiModel.CreatedAt,
                                      videoApiModel.User?.Map(),
                                      videoApiModel.Customer?.Map());
        }

        public static VideoDataModel Map(this DataApiModel<VideoApiModel> dataApiModel)
        {
            if (dataApiModel.Data is null)
            {
                return null;
            }

            return new VideoDataModel(dataApiModel.Data.Id,
                                      dataApiModel.Data.Title,
                                      dataApiModel.Data.Description,
                                      dataApiModel.Data.Poster,
                                      dataApiModel.Data.Status,
                                      dataApiModel.Data.ViewsCount ?? 0,
                                      dataApiModel.Data.RepostsCount ?? 0,
                                      dataApiModel.Data.LikesCount ?? 0,
                                      dataApiModel.Data.DislikesCount ?? 0,
                                      dataApiModel.Data.CommentsCount ?? 0,
                                      dataApiModel.Data.StreamUri,
                                      dataApiModel.Data.PreviewUri,
                                      dataApiModel.Data.MarkedStreamUri,
                                      dataApiModel.Data.ShareUri,
                                      dataApiModel.Data.IsLiked,
                                      dataApiModel.Data.IsDisliked,
                                      dataApiModel.Data.OrderCategory,
                                      dataApiModel.Data.CreatedAt,
                                      dataApiModel.Data.User?.Map(),
                                      dataApiModel.Data.Customer?.Map());
        }
    }
}