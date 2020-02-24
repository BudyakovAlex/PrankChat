﻿using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.CrashlyticService;
using PrankChat.Mobile.iOS.ApplicationServices;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Crashlytic;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Video;
using PrankChat.Mobile.iOS.Presentation.Binding;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IVideoPlayerService, VideoPlayerService>();
            Mvx.IoCProvider.RegisterType<ICrashlyticsService, CrashlyticsService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(typeof(UIButtonSelectedTargetBinding), typeof(UIButton), UIButtonSelectedTargetBinding.TargetBinding);
            registry.RegisterPropertyInfoBindingFactory(typeof(WebViewUrlTargetBinding), typeof(WKWebView), WebViewUrlTargetBinding.TargetBinding);
            base.FillTargetFactories(registry);
        }
    }
}
