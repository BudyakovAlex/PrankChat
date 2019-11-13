﻿using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity]
    public class PasswordRecoveryView : BaseView
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.password_recovery_layout);
        }
    }
}
