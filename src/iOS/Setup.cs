﻿using AVFoundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using PrankChat.Mobile.Core;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.Platform;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Services.ExternalAuth;
using PrankChat.Mobile.Core.Services.FileSystem;
using PrankChat.Mobile.Core.Services.Network;
using PrankChat.Mobile.iOS.Binding;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.PlatformBusinessServices.FileSystem;
using PrankChat.Mobile.iOS.Plugins.HttpClient;
using PrankChat.Mobile.iOS.Plugins.Logging;
using PrankChat.Mobile.iOS.Plugins.UserInteraction;
using PrankChat.Mobile.iOS.Plugins.Video;
using PrankChat.Mobile.iOS.Services.ExternalAuth;
using PrankChat.Mobile.iOS.Services.ExternalAuth.AppleSignIn;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using UIKit;
using WebKit;

namespace PrankChat.Mobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        private const string LoggerTag = "Prank_IOS";

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);

            Mvx.IoCProvider.CallbackWhenRegistered<IEnvironmentConfigurationProvider>(provider =>
            {
                AppCenter.Start(provider.Environment.AppCenterIosId, typeof(Analytics), typeof(Crashes));
            });
        }

        public override void InitializeSecondary()
        {
            //NOTE: need to trace errors in console
            try
            {
                base.InitializeSecondary();
            }
            catch (Exception exception)
            {
                NativeConsoleLogger.Write(LoggerTag, $"{exception.Message} \n\n\n\n{exception.StackTrace}");
            }
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Debug()
                .CreateLogger();
            return new SerilogLoggerFactory();
        }

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeLastChance(iocProvider);

            CompositionRoot.Container.RegisterType<IVideoPlayer, VideoPlayer>();
            CompositionRoot.Container.RegisterSingleton<IUserInteraction, UserInteraction>();
            CompositionRoot.Container.RegisterSingleton<IFileSystemService, FileSystemService>();
            CompositionRoot.Container.RegisterSingleton<IExternalAuthService, ExternalAuthService>();
            CompositionRoot.Container.RegisterSingleton<IUserSessionProvider, UserSessionProvider>();
            CompositionRoot.Container.RegisterSingleton<IAppleSignInService, AppleSignInService>();
            CompositionRoot.Container.RegisterSingleton<IPlatformPathsProvider, PlatformPathsProvider>();
            CompositionRoot.Container.RegisterSingleton<IPlatformHttpClient, PlatformHttpClient>();

            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<UIButton>(nameof(UIButtonSelectedTargetBinding), v => new UIButtonSelectedTargetBinding(v));
            registry.RegisterCustomBindingFactory<UIImageView>(nameof(UIImageViewOrderTypeTargetBinding), v => new UIImageViewOrderTypeTargetBinding(v));
            registry.RegisterCustomBindingFactory<UIButton>(nameof(UIButtonOrderTypeTargetBinding), v => new UIButtonOrderTypeTargetBinding(v));
            registry.RegisterCustomBindingFactory<WKWebView>(nameof(WKWebViewHtmlStringTargetBinding), v => new WKWebViewHtmlStringTargetBinding(v));
            registry.RegisterCustomBindingFactory<UITabBarController>(nameof(UITabBarControllerIsEnabedTargetBinding), v => new UITabBarControllerIsEnabedTargetBinding(v));
            registry.RegisterCustomBindingFactory<UILabel>(nameof(UILabelAttributedTextTargetBinding), v => new UILabelAttributedTextTargetBinding(v));

            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.StartPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.TopPadding));
            registry.RegisterCustomBindingFactory<FloatPlaceholderTextField>(FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding, view => new FloatPlaceholderTextFieldPaddingTargetBinding(view, FloatPlaceholderTextFieldPaddingTargetBinding.BottomPadding));

            base.FillTargetFactories(registry);
        }
    }
}