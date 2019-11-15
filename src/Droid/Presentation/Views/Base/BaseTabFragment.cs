using MvvmCross.Droid.Support.V4;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseTabFragment<TMvxViewModel> : MvxFragment<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        public override bool UserVisibleHint
        {
            get => base.UserVisibleHint;
            set
            {
                base.UserVisibleHint = value;
                if (value)
                    ViewModel.ViewAppeared();
                else
                    ViewModel.ViewDisappeared();
            }
        }
    }
}
