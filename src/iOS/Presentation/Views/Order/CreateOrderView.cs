using System;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Create Order", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class CreateOrderView : BaseTabbedView<CreateOrderViewModel>
    {
        private const int MinimumDescriptionHeight = 80;

        private UIImage _checkedImage;
        private UIImage _uncheckedImage;
        private UITextPosition _position;
        private UITextView dynamicDescriptionTextView;
       
        public string OrderDescription
        {
            set
            {
                if (value is null)
                {
                    return;
                }

                var size = GetTextViewHeight(descriptionTextView.Bounds.Width, descriptionTextView.Font, value);
                TextViewHeightConstraint.Constant = size > MinimumDescriptionHeight ? size : MinimumDescriptionHeight;
                descriptionPlaceholderLabel.Hidden = value.Length > 0;
                descriptionTopFloatingPlaceholderLabel.Hidden = value.Length == 0;
            }
        }

        protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<CreateOrderView, CreateOrderViewModel>();

            set.Bind(nameTextField)
               .To(vm => vm.Title);

            set.Bind(descriptionTextView)
               .To(vm => vm.Description);

            set.Bind(this)
               .For(nameof(OrderDescription))
               .To(vm => vm.Description);

            set.Bind(priceTextField)
               .To(vm => vm.Price)
               .WithConversion<PriceConverter>();

            set.Bind(completeDateTextField)
               .To(vm => vm.ActiveFor.Title);

            set.Bind(completeDateTextField.Tap())
               .For(v => v.Command)
               .To(vm => vm.ShowDateDialogCommand);

            set.Bind(createButton)
               .To(vm => vm.CreateCommand);

            set.Bind(progressBarView)
               .For(v => v.BindVisible())
               .To(vm => vm.IsBusy);

            set.Apply();
		}

        private nfloat GetTextViewHeight(double width, UIFont font, string text)
        {
            dynamicDescriptionTextView.Frame = new CoreGraphics.CGRect(0, 0, width, double.MaxValue);
            dynamicDescriptionTextView.Font = font;
            dynamicDescriptionTextView.Text = text;
            dynamicDescriptionTextView.SizeToFit();

            return dynamicDescriptionTextView.Frame.Height;
        }

        protected override void SetupControls()
		{
            descriptionTextView.TextContainer.MaximumNumberOfLines = 100;
            descriptionContainerView.Layer.BorderColor = Theme.Color.TextFieldDarkBorder.CGColor;
            descriptionContainerView.Layer.BorderWidth = 1f;
            descriptionContainerView.Layer.CornerRadius = 3f;
            dynamicDescriptionTextView = new UITextView();
            DefinesPresentationContext = true;

            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                NavigationItemHelper.CreateBarButton("ic_info", ViewModel.ShowWalkthrouthCommand)
            }, true);

            _checkedImage = UIImage.FromBundle("ic_checkbox_checked");
            _uncheckedImage = UIImage.FromBundle("ic_checkbox_unchecked");

            Title = Resources.CreateOrderView_Title;

            nameTextField.SetDarkStyle(Resources.CreateOrderView_Name_Placeholder);

            descriptionTextView.SetTitleStyle(size:14);
            descriptionTextView.ContentInset = UIEdgeInsets.Zero;
            descriptionPlaceholderLabel.SetSmallSubtitleStyle(Resources.CreateOrderView_Description_Placeholder, 14);
            descriptionTopFloatingPlaceholderLabel.SetSmallSubtitleStyle(Resources.CreateOrderView_Description_Placeholder);
            descriptionTopFloatingPlaceholderLabel.Hidden = true;

            priceTextField.SetDarkStyle(Resources.CreateOrderView_Price_Placeholder, rightPadding: 14);
            priceTextField.TextAlignment = UITextAlignment.Right;

            completeDateTextField.SetDarkStyle(Resources.CreateOrderView_CompleteDate_Placeholder, rightImage: UIImage.FromBundle("ic_calendar_accent"));

            UpdateCheckboxState();
            hideExecuterCheckboxImageView.AddGestureRecognizer(new UITapGestureRecognizer(OnCheckboxTapped));
            hideExecuterCheckboxImageView.UserInteractionEnabled = true;

            //TODO: Hide executor functionality not implemented.
            hideExecuterCheckboxImageView.Hidden = true;
            hideExecutorCheckboxLabel.Hidden = true;

            hideExecutorCheckboxLabel.Text = Resources.CreateOrderView_HideExecutor_Button;
            hideExecutorCheckboxLabel.SetRegularStyle(14, Theme.Color.Black);
            hideExecutorCheckboxLabel.UserInteractionEnabled = true;
            hideExecutorCheckboxLabel.AddGestureRecognizer(new UITapGestureRecognizer(OnCheckboxTapped));

            createButton.SetDarkStyle(Resources.CreateOrderView_Create_Button);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            stackView.SetCustomSpacing(8, stackView.ArrangedSubviews[0]);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            views.Add(stackView);

            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(nameTextField);
            viewList.Add(descriptionTextView);
            viewList.Add(completeDateTextField);
            viewList.Add(priceTextField);

            descriptionTextView.ScrollEnabled = false;

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        protected override void Subscription()
        {
            priceTextField.EditingChanged += PriceTextField_EditingChanged;
        }

        protected override void Unsubscription()
        {
            priceTextField.EditingChanged -= PriceTextField_EditingChanged;
        }

        private void PriceTextField_EditingChanged(object sender, System.EventArgs e)
        {
            var text = priceTextField.Text;
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (text.EndsWith(Resources.Currency))
            {
                var position = priceTextField.GetPosition(priceTextField.EndOfDocument, -2);
                if (_position == position)
                    return;

                _position = position;
                priceTextField.SelectedTextRange = priceTextField.GetTextRange(_position, _position);
            }
        }

        private void OnCheckboxTapped()
        {
            ViewModel.IsExecutorHidden = !ViewModel.IsExecutorHidden;
            UpdateCheckboxState();
        }

        private void UpdateCheckboxState()
        {
            hideExecuterCheckboxImageView.Image = ViewModel.IsExecutorHidden ? _checkedImage : _uncheckedImage;
        }
    }
}
