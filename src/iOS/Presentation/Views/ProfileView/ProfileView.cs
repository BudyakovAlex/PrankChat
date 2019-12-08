using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Publication;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxTabPresentation(TabName = "Profile", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class ProfileView : BaseTabbedView<ProfileViewModel>
    {
        public PublicationTableSource PublicationTableSource { get; private set; }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            set.Bind(profileImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileDescriptionLabel)
                .To(vm => vm.Description)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(priceLabel)
                .To(vm => vm.Price);

            set.Bind(ordersValueLabel)
                .To(vm => vm.OrdersValue);

            set.Bind(completedOrdersValueLabel)
                .To(vm => vm.CompletedOrdersValue);

            set.Bind(subscribersValueLabel)
                .To(vm => vm.SubscribersValue);

            set.Bind(subscriptionsValueLabel)
                .To(vm => vm.SubscriptionsValue);

            set.Bind(PublicationTableSource)
                .To(vm => vm.Items);

            set.Apply();
        }

        protected override void SetupControls()
        {
            Title = ViewModel.ProfileName;

            NavigationItem.SetRightBarButtonItem(NavigationItemHelper.CreateBarButton("ic_menu", ViewModel.ShowMenuCommand), false);

            profileDescriptionLabel.SetTitleStyle();
            priceLabel.SetSmallSubtitleStyle();
            refillButton.SetDarkStyle(Resources.ProfileView_Refill);
            withdrawalButton.SetBorderlessStyle(Resources.ProfileView_Withdrawal);
            ordersValueLabel.SetBoldTitleStyle();
            ordersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Orders_Subtitle);
            completedOrdersValueLabel.SetBoldTitleStyle();
            completedOrdersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Completed_Subtitle);
            subscribersValueLabel.SetBoldTitleStyle();
            subscribersTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscribers_Subtitle);
            subscriptionsValueLabel.SetBoldTitleStyle();
            subscriptionsTitleLabel.SetSmallSubtitleStyle(Resources.ProfileView_Subscriptions_Subtitle);

            segmentedControl.SetPublicationSegmentedControlStyle(new string[] {
                Resources.ProfileView_MyOrders_Tab,
                Resources.ProfileView_CompletedOrders_Tab
            });

            segmentedControl.SelectedSegment = 0;

            PublicationTableSource = new PublicationTableSource(tableView);
            tableView.Source = PublicationTableSource;
            tableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = PublicationItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            tableView.ContentInset = new UIEdgeInsets(10, 0, 0, 0);

            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }
    }
}

