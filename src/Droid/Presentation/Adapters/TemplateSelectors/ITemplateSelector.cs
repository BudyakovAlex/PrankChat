using System;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors
{
    public interface ITemplateSelector : IMvxTemplateSelector
    {
        Type GetItemViewHolderType(int templateId);
    }
}
