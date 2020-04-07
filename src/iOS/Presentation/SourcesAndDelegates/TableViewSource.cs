using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates
{
    public class TableViewSource : PagedTableViewSource
    {
        private readonly Dictionary<Type, string> _reuseIdentifierDictionary = new Dictionary<Type, string>();

        public TableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public TableViewSource Register<TViewModel>(UINib nib, string reuseIdentifier)
            where TViewModel : class
        {
            _reuseIdentifierDictionary.Add(typeof(TViewModel), reuseIdentifier);
            TableView.RegisterNibForCellReuse(nib, reuseIdentifier);

            return this;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var reuseIdentifier = GetReuseIdentifier(item);
            return tableView.DequeueReusableCell(reuseIdentifier, indexPath);
        }

        private string GetReuseIdentifier(object item) =>
            _reuseIdentifierDictionary[item.GetType()];
    }
}
