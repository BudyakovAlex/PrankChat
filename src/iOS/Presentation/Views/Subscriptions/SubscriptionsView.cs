using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Subscriptions.Items;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Subscriptions
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class SubscriptionsView : BaseGradientBarView<SubscriptionsViewModel>
    {
        private TabItemView _subscribersTab;
        private TabItemView _subscriptionsTab;
        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;
        private SubscriptionTabType _tabType;

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
    }
}
