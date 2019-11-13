using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.LoginView
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class LoginView : BaseView<LoginViewModel>
    {
    }
}

