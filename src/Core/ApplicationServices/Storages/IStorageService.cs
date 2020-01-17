using System;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Storages
{
    public interface IStorageService
    {
        UserDataModel User { get; set; }
    }
}
