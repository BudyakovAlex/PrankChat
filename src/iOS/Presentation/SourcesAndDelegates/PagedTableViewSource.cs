using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates
{
    public abstract class PagedTableViewSource : MvxTableViewSource
    {
        private const double VisibleCellsMultiplier = 0.7;

        protected PagedTableViewSource(UITableView tableView) : base(tableView)
        {
        }

        public ICommand LoadNextPageCommand { get; set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (CanLoadNextPage(indexPath.Row))
            {
                LoadNextPageCommand?.Execute(null);
            }

            return base.GetCell(tableView, indexPath);
        }

        protected virtual bool CanLoadNextPage(int lastVisiblePosition)
        {
            return ItemsSource.Count() * VisibleCellsMultiplier < lastVisiblePosition;
        }
    }
}