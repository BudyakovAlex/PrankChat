﻿using MvvmCross.Droid.Support.V4;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Droid.Presentation.Views.Base
{
    public abstract class BaseFragment<TMvxViewModel> : MvxFragment<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
    }
}