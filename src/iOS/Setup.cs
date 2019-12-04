﻿using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;

namespace PrankChat.Mobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
        }
    }
}
