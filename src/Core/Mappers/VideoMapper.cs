using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class VideoMapper
    {
        public static VideoDataModel Map(this VideoApiModel videoApiModel)
        {
            return new VideoDataModel(videoApiModel.Id,
                              videoApiModel.Title,
                              videoApiModel.Description,
                              videoApiModel.Poster,
                              videoApiModel.Status,
                              videoApiModel.ViewsCount,
                              videoApiModel.RepostsCount,
                              videoApiModel.LikesCount,
                              videoApiModel.DislikesCount,
                              videoApiModel.CommentsCount,
                              videoApiModel.StreamUri,
                              videoApiModel.PreviewUri,
                              videoApiModel.MarkedStreamUri,
                              videoApiModel.ShareUri,
                              videoApiModel.IsLiked,
                              videoApiModel.IsDisliked,
                              videoApiModel.OrderCategory,
                              videoApiModel.CreatedAt,
                              videoApiModel.User.Map(),
                              videoApiModel.Customer.Map());
        }
    }
}
