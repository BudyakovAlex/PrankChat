using System;

namespace PrankChat.Mobile.Core.Configuration
{
    internal interface IConfiguration
    {
        string BaseAddress { get; }

        Version ApiVersion { get; }
    }
}
