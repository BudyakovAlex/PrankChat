using System;

namespace PrankChat.Mobile.Core.Configuration
{
    internal interface IApplicationConfiguration
    {
        string BaseAddress { get; }

        Version ApiVersion { get; }
    }
}
