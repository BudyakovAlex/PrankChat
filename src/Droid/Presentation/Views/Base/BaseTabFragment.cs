using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseTabFragment<TMvxViewModel> : BaseFragment<TMvxViewModel> where TMvxViewModel : BasePageViewModel
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
    }
}
