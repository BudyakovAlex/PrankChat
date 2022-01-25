using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class MyCompetitionsView : BaseViewController<MyCompetitionsViewModel>
    {
        private TabItemView _orderedTab;
        private TabItemView _inExecuteTab;

        private TableViewSource _source;
        private MvxUIRefreshControl _refreshControl;
        private CompetitionsTabType _tabType;
        private EmptyView _emptyView;

        public CompetitionsTabType TabType
        {
            get => _tabType;
            set
            {
                _tabType = value;
                TabView.SelectedTab = _tabType == CompetitionsTabType.Ordered
                    ? _orderedTab
                    : _inExecuteTab;
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

            using var bindingSet = this.CreateBindingSet<MyCompetitionsView, MyCompetitionsViewModel>();

            bindingSet.Bind(this).For(v => v.TabType).To(vm => vm.SelectedTabType);
            bindingSet.Bind(this).For(v => v.Title).To(vm => vm.Title);
            bindingSet.Bind(_orderedTab).For(v => v.BindTitle()).To(vm => vm.OrderedTitle);
            bindingSet.Bind(_inExecuteTab).For(v => v.BindTitle()).To(vm => vm.OnExecutionTitle);
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
        }

        private void InitializeTabView()
        {
            _orderedTab = new TabItemView(string.Empty, () => ViewModel.SelectedTabType = CompetitionsTabType.Ordered);
            _inExecuteTab = new TabItemView(string.Empty, () => ViewModel.SelectedTabType = CompetitionsTabType.InExecution);

            TabView.AddTab(_orderedTab);
            TabView.AddTab(_inExecuteTab);
        }

        private void InitializeTableView()
        {
            _source = new TableViewSource(TableView)
                .Register<CompetitionItemViewModel>(SubscriptionItemCell.Nib, SubscriptionItemCell.CellId);

            _refreshControl = new MvxUIRefreshControl();
            TableView.RefreshControl = _refreshControl;

            TableView.Source = _source;
            TableView.RowHeight = SubscriptionItemCell.Height;
            TableView.ContentInset = new UIEdgeInsets(8f, 0f, 8f, 0f);
        }

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(Resources.CompetitionsListIsEmpty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(TableView);
        }
    }
}