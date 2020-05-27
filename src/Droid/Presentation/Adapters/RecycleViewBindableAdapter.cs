using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using Object = Java.Lang.Object;

namespace PrankChat.Mobile.Droid.Presentation.Adapters
{
    public class RecycleViewBindableAdapter : MvxRecyclerAdapter
    {
        private RecyclerView.RecycledViewPool _viewPool;
        private RecyclerView.RecycledViewPool ViewPool =>
            _viewPool ?? (_viewPool = new RecyclerView.RecycledViewPool());

        private readonly Dictionary<int, int> _maxRecycledViewsCount = new Dictionary<int, int>();
        private readonly Action<Object> _onViewAttachedToWindow;
        private readonly Action<Object> _onViewDetachedFromWindow;

        public RecycleViewBindableAdapter(
            IMvxAndroidBindingContext bindingContext,
            Action<Object> onViewAttachedToWindow = null,
            Action<Object> onViewDetachedFromWindow = null)
            : base(bindingContext)
        {
            _onViewAttachedToWindow = onViewAttachedToWindow;
            _onViewDetachedFromWindow = onViewDetachedFromWindow;
        }

        protected RecycleViewBindableAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (!(ItemTemplateSelector is ITemplateSelector))
            {
                throw new ArgumentException("ItemTemplateSelector must implement ITemplateSelector");
            }

            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var templateSelector = (ITemplateSelector)ItemTemplateSelector;
            var viewHolder = Activator.CreateInstance(templateSelector.GetItemViewHolderType(viewType),
                                                      itemBindingContext.BindingInflate(viewType, parent, false),
                                                      itemBindingContext) as CardViewHolder;

            viewHolder.ThrowIfNull();

            if (viewHolder is INestedCardViewHolder nestedHolder)
            {
                _maxRecycledViewsCount.TryGetValue(viewType, out var currentMaxCount);
                _maxRecycledViewsCount[viewType] = currentMaxCount + nestedHolder.RecycledViewsVisibleCount;
                ViewPool.SetMaxRecycledViews(viewType, _maxRecycledViewsCount[viewType]);
                nestedHolder.NestedRecyclerView.SetRecycledViewPool(ViewPool);
            }

            return viewHolder;
        }

        public override void OnViewAttachedToWindow(Object holder)
        {
            _onViewAttachedToWindow?.Invoke(holder);
            base.OnViewAttachedToWindow(holder);
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            _onViewDetachedFromWindow?.Invoke(holder);
            base.OnViewDetachedFromWindow(holder);
        }
    }
}
