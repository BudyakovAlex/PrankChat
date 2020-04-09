using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using MvvmCross;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Droid.PlatformBusinessServices.Notifications
{
	[Service]
	public class NotificationActionService : IntentService
	{
        private int? _orderId;

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

            _orderId = NotificationWrapper.GetOrderId(intent);
			Core.ApplicationServices.Notifications.NotificationManager.Instance.TryNavigateToView(_orderId);
        }
    }
}
