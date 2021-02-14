using System;
using System.Collections.Generic;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Configuration
{
    internal interface IApplicationConfiguration
    {
        string BaseAddress { get; }

        Version ApiVersion { get; }

        List<Period> Periods { get; }
    }
}
