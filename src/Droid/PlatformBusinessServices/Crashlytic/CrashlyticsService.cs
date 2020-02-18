using System;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Crashlytic
{
    public class CrashlyticsService : ICrashlyticsService
    {
		public void TrackEvent(string message)
		{
			Crashlytics.Crashlytics.Log(message);
		}

		public void TrackError(Exception exception)
		{
			Crashlytics.Crashlytics.LogException(Java.Lang.Throwable.FromException(exception));
		}
	}
}
