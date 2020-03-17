using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors
{
    public class TemplateSelector : ITemplateSelector
    {
        private readonly Dictionary<Type, int> ItemTypeToViewTypeIdMappings = new Dictionary<Type, int>();
        private readonly Dictionary<int, Type> ResourceIdToViewHolderTypeMappings = new Dictionary<int, Type>();
        private readonly Dictionary<int, int> ViewTypeIdToResourceIdMappings = new Dictionary<int, int>();

        public int ItemTemplateId { get; set; }

        public TemplateSelector()
        {
        }

        public TemplateSelector(IEnumerable<TemplateSelectorItem> items)
        {
            items.ThrowIfNull();

            if (items is null || items.Any())
            {
                throw new ArgumentException("items cannot be null or empty");
            }

            foreach (var item in items)
            {
                AddElement(item);
            }
        }

        public TemplateSelector(TemplateSelectorItem item) : this(new TemplateSelectorItem[] { item })
        {
        }

        public TemplateSelector(int resourceId, Type itemType, Type viewHolderType) : this(new TemplateSelectorItem[] { new TemplateSelectorItem(resourceId, itemType, viewHolderType) })
        {
        }

        public TemplateSelector AddElement<TItemType, TViewHolderType>(int resourceId)
            where TItemType : MvxNotifyPropertyChanged
            where TViewHolderType : CardViewHolder
        {
            var element = TemplateSelectorItem.Produce<TItemType, TViewHolderType>(resourceId);
            AddElement(element);

            return this;
        }

        public void AddElement(TemplateSelectorItem item)
        {
            item.ThrowIfNull();

            item.ItemType.Name.ThrowIfNull();
            item.ViewHolderType.Name.ThrowIfNull();

            var viewTypeId = $"{item.ItemType.Name}.{item.ViewHolderType}".GetHashCode();

            ItemTypeToViewTypeIdMappings.TryAdd(item.ItemType, viewTypeId);
            ResourceIdToViewHolderTypeMappings.TryAdd(item.ResourceId, item.ViewHolderType);
            ViewTypeIdToResourceIdMappings.TryAdd(viewTypeId, item.ResourceId);
        }

        public virtual int GetItemLayoutId(int fromViewType)
        {
            var hasMapping = ViewTypeIdToResourceIdMappings.TryGetValue(fromViewType, out var resourceId);
            if (!hasMapping)
            {
                throw new ArgumentOutOfRangeException($"ViewTypeIdToResourceIdMappings doesn't contain key {fromViewType}");
            }

            return resourceId;
        }

        public virtual int GetItemViewType(object forItemObject)
        {
            var hasMapping = ItemTypeToViewTypeIdMappings.TryGetValue(forItemObject.GetType(), out var viewTypeId);
            if (!hasMapping)
            {
                throw new ArgumentOutOfRangeException($"ItemTypeToViewTypeIdMappings doesn't contain key {forItemObject.GetType()}");
            }

            return viewTypeId;
        }

        public virtual Type GetItemViewHolderType(int fromViewType)
        {
            var hasMapping = ResourceIdToViewHolderTypeMappings.TryGetValue(fromViewType, out var viewHolderType);
            if (!hasMapping)
            {
                throw new ArgumentOutOfRangeException($"ViewTypeIdToViewHolderTypeMappings doesn't contain key {fromViewType}");
            }

            return viewHolderType;
        }
    }
}
