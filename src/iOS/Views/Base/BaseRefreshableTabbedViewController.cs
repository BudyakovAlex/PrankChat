using Foundation;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseRefreshableTabbedViewController<TViewModel> : BaseTabbedViewController<TViewModel>, IRefreshableView
        where TViewModel : BasePageViewModel
    {
        private NSObject _backgroundNotificationObserver;
        private NSObject _foregroundNotificationObserver;
        private DateTime _timeStamp;

        private bool _isDisposed;

        public BaseRefreshableTabbedViewController()
        {
            _backgroundNotificationObserver = UIApplication.Notifications.ObserveDidEnterBackground(OnDidEnterBackground);
            _foregroundNotificationObserver = UIApplication.Notifications.ObserveWillEnterForeground(OnWillEnterForeground);
        }

        protected abstract void RefreshData();

        private void OnWillEnterForeground(object sender, NSNotificationEventArgs args)
        {
            var timeSpan = DateTime.Now - _timeStamp;
            if (timeSpan.Hours < 1)
            {
                return;
            }

            RefreshData();
        }

        private void OnDidEnterBackground(object sender, NSNotificationEventArgs args)
        {
            _timeStamp = DateTime.Now;
        }

        void IRefreshableView.RefreshData()
        {
            RefreshData();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_backgroundNotificationObserver);
                NSNotificationCenter.DefaultCenter.RemoveObserver(_foregroundNotificationObserver);
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }
    }
}