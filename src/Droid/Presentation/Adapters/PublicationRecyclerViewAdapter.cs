using System;
using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications;

namespace PrankChat.Mobile.Droid.Presentation.Adapters
{
    public class PublicationRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly IMvxAndroidBindingContext _bindingContext;

        private MvxObservableCollection<PublicationItemViewModel> _itemsSource;
        private IDisposable _collectionChangedSubscription;

        public PublicationRecyclerViewAdapter(IMvxAndroidBindingContext bindingContext)
        {
            _bindingContext = bindingContext ?? MvxAndroidBindingContextHelpers.Current();
        }

        public MvxObservableCollection<PublicationItemViewModel> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (_itemsSource == value || value == null)
                {
                    return;
                }

                _itemsSource = value;

                _collectionChangedSubscription?.Dispose();
                _collectionChangedSubscription = null;
                _collectionChangedSubscription = _itemsSource.WeakSubscribe(OnItemsSourceCollectionChanged);

                NotifyDataSetChanged();
            }
        }

        public override int ItemCount => ItemsSource.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewModel = ItemsSource[position];
            var viewHolder = (PublicationItemViewHolder)holder;
            viewHolder.ViewModel = viewModel;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var context = new MvxAndroidBindingContext(parent.Context, _bindingContext.LayoutInflaterHolder);
            var view = context.BindingInflate(Resource.Layout.cell_publication, parent, false);
            var viewHolder = new PublicationItemViewHolder(view, context);

            return viewHolder;
        }

        protected override void Dispose(bool disposing)
        {
            _collectionChangedSubscription?.Dispose();
            _collectionChangedSubscription = null;

            base.Dispose(disposing);
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_collectionChangedSubscription == null || ItemsSource == null)
            {
                return;
            }

            NotifyDataSetChanged(e);
        }

        private void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                    break;

                case NotifyCollectionChangedAction.Move:
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        NotifyItemMoved(e.OldStartingIndex + i, e.NewStartingIndex + i);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
            }
        }
    }
}
