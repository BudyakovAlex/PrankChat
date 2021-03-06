using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Extensions.MvvmCross;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Subscriptions.Items;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using PrankChat.Mobile.iOS.Views.Subscriptions.Items;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Subscriptions
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SubscriptionsView : BaseViewController<SubscriptionsViewModel>
    {
        private TabItemView _subscribersTab;
        private TabItemView _subscriptionsTab;
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;
        private SubscriptionTabType _tabType;
        private EmptyView _emptyView;

        public SubscriptionTabType TabType
        {
            get => _tabType;
            set
            {
                _tabType = value;
                TabView.SelectedTab = _tabType == SubscriptionTabType.Subscribers
                    ? _subscribersTab
                    : _subscriptionsTab;
            }
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            InitializeTabView();
            InitializeTableView();
            CreateEmptyView();
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<SubscriptionsView, SubscriptionsViewModel>();

            bindingSet.Bind(this).For(v => v.TabType).To(vm => vm.SelectedTabType);
            bindingSet.Bind(this).For(v => v.Title).To(vm => vm.Title);
            bindingSet.Bind(_subscribersTab).For(v => v.BindTitle()).To(vm => vm.SubscribersTitle);
            bindingSet.Bind(_subscriptionsTab).For(v => v.BindTitle()).To(vm => vm.SubscriptionsTitle);
            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_source).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
            bindingSet.Bind(_emptyView)
                .For(v => v.Title)
                .To(vm => vm.SelectedTabType)
                .WithConversion((SubscriptionTabType subscriptionTabType) =>
                    subscriptionTabType == SubscriptionTabType.Subscribers
                    ? Resources.SubscribersListIsEmpty
                    : Resources.SubscriptionsListIsEmpty);
        }

        private void InitializeTabView()
        {
            _subscribersTab = new TabItemView(string.Empty, () => ViewModel.SelectedTabType = SubscriptionTabType.Subscribers);
            _subscriptionsTab = new TabItemView(string.Empty, () => ViewModel.SelectedTabType = SubscriptionTabType.Subscriptions);

            TabView.AddTab(_subscribersTab);
            TabView.AddTab(_subscriptionsTab);
        }

        private void InitializeTableView()
        {
            _source = new TableViewSource(TableView)
                .Register<SubscriptionItemViewModel>(SubscriptionItemCell.Nib, SubscriptionItemCell.CellId);

            _refreshControl = new MvxUIRefreshControl();
            TableView.RefreshControl = _refreshControl;

            TableView.Source = _source;
            TableView.RowHeight = SubscriptionItemCell.Height;
            TableView.ContentInset = new UIEdgeInsets(8f, 0f, 8f, 0f);
        }

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(string.Empty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(TableView);
        }
    }
}
