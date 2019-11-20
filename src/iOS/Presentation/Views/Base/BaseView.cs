using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetupControls();
			SetupBinding();
		}

		protected abstract void SetupBinding();

		protected abstract void SetupControls();
	}
}
