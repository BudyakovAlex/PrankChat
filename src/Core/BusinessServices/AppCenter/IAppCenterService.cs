using System;

namespace PrankChat.Mobile.Core.BusinessServices.AppCenter
{
	public interface IAppCenterService
	{
		void TrackEvent(string message);

		void TrackError(Exception exception);
	}
}
