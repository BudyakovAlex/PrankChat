using System;
using Sentry;

namespace PrankChat.Mobile.Core.BusinessServices.Sentry
{
    public class SentryService : ISentryService
    {
        public void TrackError(Exception exception)
        {
            SentrySdk.CaptureException(exception);
        }

        public void TrackEvent(string message)
        {
            SentrySdk.CaptureMessage(message);
        }
    }
}