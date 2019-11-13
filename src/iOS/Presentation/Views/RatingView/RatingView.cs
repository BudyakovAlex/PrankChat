using System;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.RatingView
{
    [MvxTabPresentation]
    public partial class RatingView : BaseView<RatingViewModel>
    {
    }
}

