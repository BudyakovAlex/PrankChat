using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.BusinessServices.Sentry
{
    public interface ISentryService
    {
        void TrackEvent(string message);

        void TrackError(Exception exception);
    }
}
