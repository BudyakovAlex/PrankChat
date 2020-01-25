using System;
namespace PrankChat.Mobile.Core.Models.Api
{
    [Flags]
    public enum IncludeType
    {
        User,
        Comments,
        Customer,
        Executor,
        Videos,
        InArbitration,
        Order
    }
}
