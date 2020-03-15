using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters
{
    public class RecycleViewBindableAdapter : MvxRecyclerAdapter
    {
        private RecyclerView.RecycledViewPool _viewPool;
        private RecyclerView.RecycledViewPool ViewPool => _viewPool ?? (_viewPool = new RecyclerView.RecycledViewPool());

        private readonly Dictionary<int, int> _maxRecycledViewsCount = new Dictionary<int, int>();

        private readonly Action<Java.Lang.Object> _onViewAttachedToWindow;
        private readonly Action<Java.Lang.Object> _onViewDetachedFromWindow;

        public RecycleViewBindableAdapter()
        {
        }

        public RecycleViewBindableAdapter(IMvxAndroidBindingContext bindingContext, Action<Java.Lang.Object> onViewAttachedToWindow = null, Action<Java.Lang.Object> onViewDetachedFromWindow = null) : base(bindingContext)
        {
            _onViewAttachedToWindow = onViewAttachedToWindow;
            _onViewDetachedFromWindow = onViewDetachedFromWindow;
        }

        public RecycleViewBindableAdapter(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            if (!(ItemTemplateSelector is ITemplateSelector))
            {
                throw new ArgumentException("ItemTemplateSelector must implement ITemplateSelector");
            }

            var templateSelector = (ITemplateSelector)ItemTemplateSelector;

            var viewHolder = Activator.CreateInstance(templateSelector.GetItemViewHolderType(viewType),
                                                      itemBindingContext.BindingInflate(viewType, parent, false),
                                                      itemBindingContext) as MvxRecyclerViewHolder;
            viewHolder.ThrowIfNull();

            viewHolder.Click += (e, a) => ItemClick?.Execute(viewHolder.DataContext);

            if (viewHolder is INestedCardViewHolder nestedHolder)
            {
                _maxRecycledViewsCount.TryGetValue(viewType, out var currentMaxCount);
                _maxRecycledViewsCount[viewType] = currentMaxCount + nestedHolder.RecycledViewsVisibleCount;
                ViewPool.SetMaxRecycledViews(viewType, _maxRecycledViewsCount[viewType]);
                nestedHolder.NestedRecyclerView.SetRecycledViewPool(ViewPool);
            }

            viewHolder.LongClick += (e, a) => ItemLongClick?.Execute(viewHolder.DataContext);
            return viewHolder;
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            _onViewAttachedToWindow?.Invoke(holder);
            base.OnViewAttachedToWindow(holder);
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            _onViewDetachedFromWindow?.Invoke(holder);
            base.OnViewDetachedFromWindow(holder);
        }
    }
}