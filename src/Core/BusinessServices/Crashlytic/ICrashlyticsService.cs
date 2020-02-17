using System;

namespace PrankChat.Mobile.Core.BusinessServices.CrashlyticService
{
	public interface ICrashlyticsService
	{
		void TrackEvent(string message);

		void TrackError(Exception exception);
	}
}
