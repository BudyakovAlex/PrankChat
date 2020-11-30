using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data.Shared;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CommentMapper
    {
        public static CommentDataModel Map(this CommentApiModel commentApiModel)
        {
            return new CommentDataModel(commentApiModel.Id,
                                commentApiModel.Text,
                                commentApiModel.CreatedAt,
                                commentApiModel.User.Map());
        }
    }
}
