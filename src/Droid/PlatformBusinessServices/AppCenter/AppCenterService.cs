using System;
using PrankChat.Mobile.Core.BusinessServices.AppCenter;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.AppCenter
{
    public class AppCenterService : IAppCenterService
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
