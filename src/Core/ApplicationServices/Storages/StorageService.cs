using System;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Storages
{
    public class StorageService : IStorageService
    {
        public UserDataModel User { get; set; }
    }
}
