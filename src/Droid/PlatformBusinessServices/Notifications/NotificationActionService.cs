using System;
using Android.App;
using Android.Content;
using Android.Runtime;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications
{
	[Service]
	public class NotificationActionService : IntentService
	{
		public NotificationActionService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public NotificationActionService(string name) : base(name)
		{
		}

		public NotificationActionService()
		{
		}

		protected override void OnHandleIntent(Intent intent)
		{
			var splashActivity = new Intent(Application.Context, typeof(SplashScreen)).AddFlags(ActivityFlags.NewTask);
			StartActivity(splashActivity);
			//TODO: should init the logic for display VM
		}
	}
}
