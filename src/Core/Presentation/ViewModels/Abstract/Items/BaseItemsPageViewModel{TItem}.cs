using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;
using PrankChat.Mobile.Core.Extensions;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract.Items
{
    public abstract class BaseItemsPageViewModel<TItem> : BasePageViewModel
    {
        private readonly SerialDisposable _itemsSubscription;

        protected BaseItemsPageViewModel()
        {
            _itemsSubscription = new SerialDisposable().DisposeWith(Disposables);
            Items = new MvxObservableCollection<TItem>();
        }

        private MvxObservableCollection<TItem> _items;
        public MvxObservableCollection<TItem> Items
        {
            get => _items;
            protected set
            {
                value.ThrowIfNull();
                if (SetProperty(ref _items, value, OnItemsChanged))
                {
                    _itemsSubscription.Disposable = value.WeakSubscribe(OnItemsCollectionChanged);
                    ExecutionStateWrapper.WrapAsync(() => RaisePropertiesChanged(nameof(Count), nameof(IsEmpty), nameof(IsNotEmpty)));
                }
            }
        }

        public int Count => Items.Count;

        public bool IsEmpty => Items.IsEmpty();

        public bool IsNotEmpty => Items.IsNotEmpty();

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ExecutionStateWrapper.WrapAsync(() => RaisePropertiesChanged(nameof(Count), nameof(IsEmpty), nameof(IsNotEmpty)));
            OnItemsCollectionChanged();
        }

        protected virtual void OnItemsChanged()
        {
        }

        protected virtual void OnItemsCollectionChanged()
        {
        }
    }
}