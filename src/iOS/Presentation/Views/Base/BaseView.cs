using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public class BaseView<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
    }
}
