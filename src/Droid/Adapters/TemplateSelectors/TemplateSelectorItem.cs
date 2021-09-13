using System;
using Java.Lang;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Adapters.TemplateSelectors
{
    public class TemplateSelectorItem
    {
        public TemplateSelectorItem(int resourceId, Type itemType, Type viewHolderType)
        {
            if (!itemType.IsSubclassOf(typeof(MvxNotifyPropertyChanged)))
            {
                throw new IllegalStateException("itemType must derived from FeedItemViewModel");
            }

            if (!viewHolderType.IsSubclassOf(typeof(CardViewHolder)) && viewHolderType != typeof(CardViewHolder))
            {
                throw new IllegalStateException("viewHolderType must derived from CardViewHolder");
            }

            ResourceId = resourceId;
            ItemType = itemType;
            ViewHolderType = viewHolderType;
        }

        public static TemplateSelectorItem Produce<TItemType, TViewHolderType>(int resourceId)
            where TItemType : MvxNotifyPropertyChanged
            where TViewHolderType : CardViewHolder
        {
            var templateSelectorItem = new TemplateSelectorItem(resourceId, typeof(TItemType), typeof(TViewHolderType));

            return templateSelectorItem;
        }

        public int ResourceId { get; }

        public Type ItemType { get; }

        public Type ViewHolderType { get; }
    }
}