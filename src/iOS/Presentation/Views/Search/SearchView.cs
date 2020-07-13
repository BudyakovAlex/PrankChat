using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.Order;
using PrankChat.Mobile.iOS.Presentation.Views.Publication;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public partial class SearchView : BaseGradientBarView<SearchViewModel>
    {
        private const int SearchBarRightPadding = 16;
        private const int BackButtonWidth = 40;

        public SearchTableSource SearchTableSource { get; private set; }

        public UISearchBar SearchBar { get; set; }

        protected override void SetupBinding()
        {
            var bindingSet = this.CreateBindingSet<SearchView, SearchViewModel>();

            bindingSet.Bind(SearchTableSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(SearchBar)
                      .For(v => v.Text)
                      .To(vm => vm.SearchValue);

            bindingSet.Bind(loadingView)
                      .For(v => v.BindVisible())
                      .To(vm => vm.IsBusy);

            bindingSet.Apply();
        }

        protected override void SetupControls()
        {
            var backButton = NavigationItemHelper.CreateBarButton("ic_back", ViewModel.GoBackCommand);

            var navigationBarWidth = NavigationController?.NavigationBar.Frame.Width;
            var searchBarWidth = navigationBarWidth - BackButtonWidth - SearchBarRightPadding;
            SearchBar = new UISearchBar(new CoreGraphics.CGRect(0, 0, searchBarWidth.Value, 28))
            {
                Placeholder = Resources.Search_View_Search_Placeholder
            };

            SearchBar.SetStyle();

            NavigationItem.LeftBarButtonItems = new UIBarButtonItem[]
            {
                backButton,
                new UIBarButtonItem(SearchBar)
            };

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            tabView.AddTab(Resources.Search_Peoples, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Users;
                tableView.RowHeight = UITableView.AutomaticDimension;
            });
            tabView.AddTab(Resources.Search_Videos, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Videos;
                tableView.RowHeight = UITableView.AutomaticDimension;
            });

            tabView.AddTab(Resources.Orders_Tab, () =>
            {
                ViewModel.SearchTabType = SearchTabType.Orders;
                tableView.RowHeight = Constants.CellHeights.OrderItemCellHeight;
            });

            SearchTableSource = new SearchTableSource(tableView);
            tableView.Source = SearchTableSource;

            tableView.RegisterNibForCellReuse(ProfileSearchItemCell.Nib, ProfileSearchItemCell.CellId);
            tableView.RegisterNibForCellReuse(OrderItemCell.Nib, OrderItemCell.CellId);
            tableView.RegisterNibForCellReuse(PublicationItemCell.Nib, PublicationItemCell.CellId);

            tableView.SetStyle();
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }
    }
}