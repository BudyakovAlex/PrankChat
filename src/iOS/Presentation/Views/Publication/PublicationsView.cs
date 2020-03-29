using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    [MvxTabPresentation(TabName = "Publications", TabIconName = "unselected", TabSelectedIconName = "selected")]
    public partial class PublicationsView : BaseTabbedView<PublicationsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;

        public PublicationTableSource PublicationTableSource { get; private set; }

        protected override void SetupBinding()
		{
			var bindingSet = this.CreateBindingSet<PublicationsView, PublicationsViewModel>();

            bindingSet.Bind(filterContainerView.Tap())
                      .For(v => v.Command)
                      .To(vm => vm.OpenFilterCommand);

            bindingSet.Bind(filterTitleLabel)
                      .To(vm => vm.ActiveFilterName);

            bindingSet.Bind(PublicationTableSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(PublicationTableSource)
                      .For(v => v.ItemsChangedInteraction)
                      .To(vm => vm.ItemsChangedInteraction);

            bindingSet.Bind(PublicationTableSource)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.ReloadItemsCommand);

            bindingSet.Apply();
		}

		protected override void SetupControls()
		{
			InitializeNavigationBar();
            InitializeTableView();

            publicationTypeStackView.SetTabsStyle(new string[] {
				Resources.Popular_Publication_Tab,
				Resources.Actual_Publication_Tab,
				Resources.MyFeed_Publication_Tab,
			}, OnTabSelected);

            topSeparatorView.BackgroundColor = Theme.Color.Separator;
            filterArrowImageView.Image = UIImage.FromBundle("ic_filter_arrow");
            filterTitleLabel.Font = Theme.Font.RegularFontOfSize(14);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            // Fix to select cell
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            // Fix to select cell
        }

        private void OnTabSelected(int position)
        {
            publicationTypeStackView.SetSelectedTabStyle(position);
            ViewModel.SelectedPublicationType = (PublicationType)position;
        }

        private void InitializeNavigationBar()
		{
			NavigationController.NavigationBar.SetNavigationBarStyle();
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                NavigationItemHelper.CreateBarButton("ic_notification", ViewModel.ShowNotificationCommand),
                NavigationItemHelper.CreateBarButton("ic_search", ViewModel.ShowSearchCommand)
            }, true);

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;
        }

        private void InitializeTableView()
        {
            PublicationTableSource = new PublicationTableSource(tableView);
            tableView.Source = PublicationTableSource;
            tableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);
            tableView.SetVideoListStyle(PublicationItemCell.EstimatedHeight);

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }
    }
}