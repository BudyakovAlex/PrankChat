using PrankChat.Mobile.Core.Data.Enums;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Services.Analytics
{
    public interface IAnalyticsService
    {
        void Track(AnalyticsEvent @event, IDictionary<string, string> properties = null);
    }
}
