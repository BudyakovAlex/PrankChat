using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseRefreshableTabFragment<TMvxViewModel> : BaseFragment<TMvxViewModel>, IRefreshableView
        where TMvxViewModel : BasePageViewModel
    {
        private DateTime _timeStamp;

        protected BaseRefreshableTabFragment(int layoutId) : base(layoutId)
        {
        }

        protected abstract void RefreshData();

        public override void OnPause()
        {
            base.OnPause();
            _timeStamp = DateTime.Now;
        }

        public override void OnResume()
        {
            base.OnResume();

            var timeSpan = DateTime.Now - _timeStamp;
            if (timeSpan.Hours < 1)
            {
                return;
            }

            RefreshData();
        }

        void IRefreshableView.RefreshData()
        {
            RefreshData();
        }
    }
}