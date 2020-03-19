using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Commands;
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

        public MvxAsyncCommand LoadMoreItemsCommand { get; set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (CanMoreItems(indexPath.Row))
            {
                LoadMoreItemsCommand?.Execute(null);
            }

            return base.GetCell(tableView, indexPath);
        }

        protected virtual bool CanMoreItems(int lastVisiblePosition)
        {
            return !LoadMoreItemsCommand.IsRunning && ItemsSource.Count() * VisibleCellsMultiplier < lastVisiblePosition;
        }
    }
}