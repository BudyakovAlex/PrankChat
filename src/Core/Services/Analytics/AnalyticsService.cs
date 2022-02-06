using PrankChat.Mobile.Core.Data.Enums;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Services.Analytics
{
    public sealed class AnalyticsService : IAnalyticsService
    {
        public void Track(AnalyticsEvent @event, IDictionary<string, string> properties = null)
        {
            var message = CreateMessage(@event);
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(message, properties);
        }

        private string CreateMessage(AnalyticsEvent @event)
        {
            return @event switch
            {
                AnalyticsEvent.RegistrationManual => "Manual registration succeeded",
                AnalyticsEvent.PaymentRefill      => "Payment refilled",

                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
