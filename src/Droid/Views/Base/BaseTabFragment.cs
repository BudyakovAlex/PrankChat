using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Droid.Views.Base
{
    public abstract class BaseTabFragment<TMvxViewModel> : BaseFragment<TMvxViewModel> where TMvxViewModel : BasePageViewModel
    {
        protected BaseTabFragment(int layoutId) : base(layoutId)
        {
        }

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
    }
}
