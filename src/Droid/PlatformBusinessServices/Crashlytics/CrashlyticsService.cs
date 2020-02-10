using System;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Crashlytics
{
    public class CrashlyticsService : ICrashlyticsService
    {
		public void TrackEvent(string message)
		{
			Crashlytics.Crashlytics.Log(message);
		}

		public void TrackError(Exception exception)
		{
			if (exception is WebViewException ex)
			{
				if (ex.Code != 0)
					Crashlytics.Crashlytics.SetInt("Error Code", ex.Code);

				if (ex.Url != null)
					Crashlytics.Crashlytics.SetString("Url", ex.Url);
			}

			Crashlytics.Crashlytics.LogException(Java.Lang.Throwable.FromException(exception));
		}
	}
}
