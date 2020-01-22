using System;
namespace PrankChat.Mobile.Core.Models.Api
{
    public interface ILoadingFileApiModel
    {
        string FilePath { get; set; }

        string FileName { get; set; }
    }
}
