using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public class BaseView<TMvxViewModel> : MvxAppCompatActivity<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
    }
}
