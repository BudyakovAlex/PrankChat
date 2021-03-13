﻿using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.BusinessServices.AppCenter;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.iOS.ApplicationServices;
using PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth;
using PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth.AppleSignIn;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.PlatformBusinessServices.AppCenter;
using PrankChat.Mobile.iOS.PlatformBusinessServices.Video;
using PrankChat.Mobile.iOS.Presentation.Binding;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            CompositionRoot.Container.RegisterType<IVideoPlayerService, VideoPlayerService>();
            CompositionRoot.Container.RegisterSingleton<IAppCenterService, AppCenterService>();
            CompositionRoot.Container.RegisterSingleton<IDialogService, DialogService>();
            CompositionRoot.Container.RegisterSingleton<IExternalAuthService, ExternalAuthService>();
            CompositionRoot.Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
            CompositionRoot.Container.RegisterSingleton<IAppleSignInService, AppleSignInService>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(typeof(UIButtonSelectedTargetBinding), typeof(UIButton), UIButtonSelectedTargetBinding.TargetBinding);
            registry.RegisterCustomBindingFactory<UIImageView>(UIImageViewOrderTypeTargetBinding.TargetBinding, v => new UIImageViewOrderTypeTargetBinding(v));
            registry.RegisterCustomBindingFactory<UIButton>(UIButtonOrderTypeTargetBinding.TargetBinding, v => new UIButtonOrderTypeTargetBinding(v));
            registry.RegisterCustomBindingFactory<WKWebView>(WKWebViewHtmlStringTargetBinding.TargetBinding, v => new WKWebViewHtmlStringTargetBinding(v));

            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding));

            base.FillTargetFactories(registry);
        }
    }
}