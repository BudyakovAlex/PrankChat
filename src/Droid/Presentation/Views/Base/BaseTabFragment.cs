using MvvmCross.Droid.Support.V4;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseTabFragment<TMvxViewModel> : BaseFragment<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        public override bool UserVisibleHint
        {
            get => base.UserVisibleHint;
            set
            {
                base.UserVisibleHint = value;
                if (value)
                {
                    ViewModel.ViewAppeared();
                }
                else
                {
                    ViewModel.ViewDisappeared();
                }
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            Subscription();
        }

        public override void OnStop()
        {
            base.OnStop();
            Unsubscription();
        }
    }
}
