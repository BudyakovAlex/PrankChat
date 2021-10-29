using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Base
{
    public abstract class BaseViewController<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BasePageViewModel
    {
        private List<UIView> _viewForKeyboardDismiss = new List<UIView>();
        private NSObject _keyBoardWillDisapear;
        private NSObject _keyboardWillAppear;
        private string _title;

        public new string Title
        {
            get => _title;
            set
            {
                _title = value;
                if (!string.IsNullOrEmpty(_title))
                {
                    SetTitle(_title);
                }
            }
        }

        public virtual bool IsRotateEnabled { get; protected set; }

        public virtual bool CanHandleKeyboardNotifications => false;

        public virtual bool SetNavigationBarStyle => true;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeKeyboardDismiss();

            SetCommonStyles();
            SetupControls();
            Bind();
            Subscription();
        }

        public override void ViewDidUnload()
        {
            Unsubscription();
            base.ViewDidUnload();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (CanHandleKeyboardNotifications)
            {
                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (CanHandleKeyboardNotifications)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.BlackOpaque;
        }

        protected bool IsTabbedView { get; set; }

        protected virtual void Bind()
        {
            // nothing do
        }

        protected virtual void SetupControls()
        {
            // nothing do
        }

        protected virtual void Subscription()
        {
            // nothing do
        }

        protected virtual void Unsubscription()
        {
            // nothing do
        }

        protected virtual void SetCommonStyles()
        {
            SetNeedsStatusBarAppearanceUpdate();

            if (SetNavigationBarStyle)
            {
                NavigationController?.NavigationBar?.SetNavigationBarStyle();
            }

            if (NavigationItem.HidesBackButton == false && !IsTabbedView)
            {
                var backButton = NavigationItemHelper.CreateBarButton(ImageNames.IconBack, ViewModel.CloseCommand, UIColor.Black);
                NavigationItem.LeftBarButtonItem = backButton;
            }
        }

        protected virtual void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            View.AddGestureRecognizer(new UITapGestureRecognizer(DismissKeyboard));
            foreach (var view in views)
            {
                view.AddGestureRecognizer(new UITapGestureRecognizer(DismissKeyboard));
            }
        }

        protected virtual void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            _viewForKeyboardDismiss = viewList;
            _viewForKeyboardDismiss
                .OfType<UIScrollView>()
                .ToList()
                .ForEach(c => c.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag | UIScrollViewKeyboardDismissMode.Interactive);
        }

        protected virtual void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
            _keyBoardWillDisapear = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            _keyboardWillAppear = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        protected virtual void UnregisterForKeyboardNotifications()
        {
            if (!IsViewLoaded)
            {
                return;
            }

            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardWillAppear);
            _keyboardWillAppear.Dispose();
            _keyboardWillAppear = null;

            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyBoardWillDisapear);
            _keyBoardWillDisapear.Dispose();
            _keyBoardWillDisapear = null;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return IsRotateEnabled ? UIInterfaceOrientationMask.All : UIInterfaceOrientationMask.Portrait;
        }

        public override bool ShouldAutorotate()
        {
            return IsRotateEnabled;
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return toInterfaceOrientation != UIInterfaceOrientation.Portrait && IsRotateEnabled;
        }

        private void SetTitle(string title)
        {
            var titleItem = new UILabel(new CoreGraphics.CGRect(0, 0, 200, 20))
            {
                Text = title,
                TextAlignment = UITextAlignment.Center
            };
            titleItem.SetScreenTitleStyle();

            var container = new UIView(new CoreGraphics.CGRect(0, 0, 200, 20))
            {
                ContentMode = UIViewContentMode.Center
            };
            container.AddSubview(titleItem);

            NavigationItem.TitleView = container;
        }

        private void InitializeKeyboardDismiss()
        {
            RegisterKeyboardDismissResponders(new List<UIView> { });
            RegisterKeyboardDismissTextFields(_viewForKeyboardDismiss);
        }

        private void DismissKeyboard()
        {
            foreach (var view in _viewForKeyboardDismiss)
            {
                if (view is UIScrollView)
                {
                    continue;
                }

                view.ResignFirstResponder();
            }
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }

            var visible = notification.Name == UIKeyboard.WillShowNotification;

            UIView.BeginAnimations("AnimateForKeyboard");
            UIView.SetAnimationBeginsFromCurrentState(true);
            UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
            UIView.SetAnimationCurve((UIViewAnimationCurve) UIKeyboard.AnimationCurveFromNotification(notification));

            var keyboardFrame = visible
                ? UIKeyboard.FrameEndFromNotification(notification)
                : UIKeyboard.FrameBeginFromNotification(notification);

            OnKeyboardChanged(visible, keyboardFrame.Height);
            UIView.CommitAnimations();
        }
    }
}
