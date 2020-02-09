﻿using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Publication;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
	[MvxTabPresentation(TabName = "Profile", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
	public partial class ProfileView : BaseTabbedView<ProfileViewModel>
	{
		private MvxUIRefreshControl _refreshControl;

		public PublicationTableSource PublicationTableSource { get; private set; }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			PublicationTableSource.Initialize();
		}

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
                .Mode(MvxBindingMode.OneWay);

			set.Bind(profileImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl)
				.WithConversion<PlaceholderImageConverter>()
				.Mode(MvxBindingMode.OneWay);	

            set.Bind(profileDescriptionLabel)
                .To(vm => vm.Name)
                .Mode(MvxBindingMode.OneWay);

            set.Bind(refillButton)
                .To(vm => vm.ShowRefillCommand);

            set.Bind(withdrawalButton)
                .To(vm => vm.ShowWithdrawalCommand);

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

            set.Bind(_refreshControl)
                .For(v => v.IsRefreshing)
                .To(vm => vm.IsBusy);

            set.Bind(_refreshControl)
                .For(v => v.RefreshCommand)
                .To(vm => vm.UpdateProfileVideoCommand);

            set.Bind(PublicationTableSource)
                .For(v => v.Segment)
                .To(vm => vm.SelectedPublicationType)
                .WithConversion<PublicationTypeConverter>();

            set.Bind(profileImageView.Tap())
                .For(v => v.Command)
                .To(vm => vm.ShowUpdateProfileCommand);

            set.Bind(profileShortNameLabel)
                .To(vm => vm.ProfileShortName);

            set.Bind(profileShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ProfilePhotoUrl);

            set.Apply();
        }

        protected override void SetupControls()
        {
            InitializeTableView();

            Title = ViewModel.Name;

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

            segmentedControl.SetStyle(new string[] {
                Resources.ProfileView_MyOrders_Tab,
                Resources.ProfileView_CompletedOrders_Tab
            });

            segmentedControl.SelectedSegment = 0;
            segmentedControl.ValueChanged += SegmentedControl_ValueChanged;
        }

        private void SegmentedControl_ValueChanged(object sender, System.EventArgs e)
        {
            switch (segmentedControl.SelectedSegment)
            {
                case 0:
                    ViewModel.SelectedPublicationType = PublicationType.MyVideosOfCreatedOrders;
                    break;

                case 1:
                    ViewModel.SelectedPublicationType = PublicationType.CompletedVideosAssignmentsByMe;
                    break;
            }
        }

        private void InitializeTableView()
        {
            PublicationTableSource = new PublicationTableSource(tableView, ViewModel);
            tableView.Source = PublicationTableSource;
            tableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);
            tableView.SetVideoListStyle(PublicationItemCell.EstimatedHeight);
            tableView.ContentInset = new UIEdgeInsets(10, 0, 0, 0);

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }
    }
}

