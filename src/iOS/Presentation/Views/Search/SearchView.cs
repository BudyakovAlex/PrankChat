using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public partial class SearchView : BaseView<SearchViewModel>
    {
        private const int SearchBarRightPadding = 16;
        private const int BackButtonWidth = 40;

        public SearchTableSource SearchTableSource { get; private set; }

        public UISearchBar SearchBar { get; set; }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<SearchView, SearchViewModel>();

            set.Bind(SearchTableSource)
                .To(vm => vm.Items);

            set.Bind(SearchBar)
                .For(v => v.Text)
                .To(vm => vm.SearchValue);

            set.Apply();
        }

        protected override void SetupControls()
        {
            var backButton = NavigationItemHelper.CreateBarButton("ic_back", ViewModel.GoBackCommand);

            var navigationBarWidth = NavigationController?.NavigationBar.Frame.Width;
            var searchBarWidth = navigationBarWidth - BackButtonWidth - SearchBarRightPadding;
            SearchBar = new UISearchBar(new CoreGraphics.CGRect(0, 0, searchBarWidth.Value, 28));
            SearchBar.Placeholder = Resources.Search_View_Search_Placeholder;
            SearchBar.SetStyle();

            NavigationItem.LeftBarButtonItems = new UIBarButtonItem[]
            {
                backButton,
                new UIBarButtonItem(SearchBar)
            };

            SearchTableSource = new SearchTableSource(tableView);
            tableView.Source = SearchTableSource;
            tableView.RegisterNibForCellReuse(ProfileSearchItemCell.Nib, ProfileSearchItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = ProfileSearchItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }
    }
}

